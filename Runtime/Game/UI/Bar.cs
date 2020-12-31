using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityCommons {
	public class Bar : MonoBehaviour {
		[Header("References"), HideInInspector, SerializeField]
		private bool __DummyMember_1;

		[Required("The Bar script requires at least a main fill image.")]
		public Image MainFillImage;
		[ShowIf("UseSecondaryFillImage"), Required("The Bar script requires a secondary fill image if `Use Secondary Fill Image` is active.")]
		public Image SecondaryFillImage;
		[ShowIf("UseText"), Required("The Bar script requires a Text reference if `Use Text` is active.")]
		public TextMeshProUGUI Text;

		[Header("Settings")]
		public bool UseSecondaryFillImage = true;
		public bool UseText = true;
		[ShowIf("UseText"),
		 ValidateInput("ValidateFormat", "Text Format is not valid. Make sure it contains at least the `%1` modifier.\n%1 = current value\n%2 = max value\n%1:N = `N` digits")]
		public string TextFormat = "%1 / %2";

		[Foldout("Values")] public float MaxValue = 100f;
		[Foldout("Values")] public float InitialValue = 100f;
		[Foldout("Values"), Label("Animation Time - Duration (s)"), Space]
		public float AnimationTime = 1f;
		[Foldout("Values"), Label("Animation Time - Type")]
		public BarTimeType TimeType = BarTimeType.SecondsTotal;

		[Foldout("Colors"), Label("Default - Main Bar")]
		public Color DefaultColor = new Color(0x32 / 255.0f, 0xD6 / 255.0f, 0x25 / 255.0f);
		[Foldout("Colors"), Label("Positive Change - Main Bar")]
		public Color PositiveChangeColorMain = new Color(0.87f, 0.95f, 0.94f);
		[Foldout("Colors"), Label("Positive Change - Secondary Bar")]
		public Color PositiveChangeColorSecondary = new Color(0x32 / 255.0f, 0xD6 / 255.0f, 0x25 / 255.0f);
		[Foldout("Colors"), Label("Negative Change - Main Bar")]
		public Color NegativeChangeColorMain = new Color(0.839f, 0.219f, 0.043f);
		[Foldout("Colors"), Label("Negative Change - Secondary Bar")]
		public Color NegativeChangeColorSecondary = new Color(0.87f, 0.95f, 0.94f);

		[Foldout("Events")] public UnityEvent<float, float> OnValueChangedEvent;
		[Foldout("Events")] public UnityEvent OnBarEmpty;
		[Foldout("Events")] public UnityEvent OnBarFull;

		private float value;

		private float animationTimer;
		private float animationTime;
		private float animationStart;
		private float animationTarget;
		private float currentAnimationValue;
		private bool isAnimating;
		private bool isAnimatingPositive;

		private void Start() {
			Setup();
		}

		private void Setup() {
			AnimateFillAmount(InitialValue);
			value = InitialValue;
		}

		private void Update() {
			if (!isAnimating) return;

			// Animate
			if (isAnimatingPositive) currentAnimationValue = Mathf.Lerp(animationStart, animationTarget, animationTimer / animationTime);
			else currentAnimationValue = Mathf.Lerp(animationTarget, animationStart, 1f - animationTimer / animationTime);

			if (isAnimatingPositive) MainFillImage.fillAmount = currentAnimationValue / MaxValue;
			else SecondaryFillImage.fillAmount = currentAnimationValue / MaxValue;

			// Update timer
			animationTimer += Time.deltaTime;
			if (animationTimer >= animationTime) {
				isAnimating = false;

				SetFillAmount(animationTarget);
				MainFillImage.color = DefaultColor;
			}
		}

		private void AnimateFillAmount(float newValue) {
			if (isAnimating) AnimateFillAmount_Animating(newValue);
			else AnimateFillAmount_NotAnimating(newValue);
		}

		private void AnimateFillAmount_Animating(float newValue) {
			if (UseText) Text.text = FormatText(newValue);

			// Adjust according to difference. Only applies if TimeType is SecondsPerFillPercentage
			var percentages = 100f * (newValue - value) / MaxValue;
			var newTime = animationTime + (isAnimatingPositive ? 1 : -1) * percentages * AnimationTime;

			animationTarget = newValue;

			// Update fill
			if (isAnimatingPositive) {
				if (UseSecondaryFillImage) SecondaryFillImage.fillAmount = newValue / MaxValue;
			}
			else if (!isAnimatingPositive) {
				MainFillImage.fillAmount = newValue / MaxValue;
			}

			if (isAnimatingPositive && animationTarget < currentAnimationValue) {
				// Change colors to negative
				MainFillImage.fillAmount = newValue / MaxValue;
				MainFillImage.color = NegativeChangeColorMain;
				if (UseSecondaryFillImage) SecondaryFillImage.color = NegativeChangeColorSecondary;
				isAnimatingPositive = false;

				// Update time
				if (TimeType == BarTimeType.SecondsPerFillPercentage) {
					var percentageDifference = 100f * (currentAnimationValue - animationTarget) / MaxValue;
					newTime = percentageDifference * AnimationTime;
					animationTimer = 0f;
				}
			}
			else if (!isAnimatingPositive && animationTarget > currentAnimationValue) {
				// Change colors to positive
				if (UseSecondaryFillImage) SecondaryFillImage.fillAmount = newValue / MaxValue;
				MainFillImage.color = PositiveChangeColorMain;
				if (UseSecondaryFillImage) SecondaryFillImage.color = PositiveChangeColorSecondary;
				isAnimatingPositive = true;

				// Update time
				if (TimeType == BarTimeType.SecondsPerFillPercentage) {
					var percentageDifference = 100f * (animationTarget - currentAnimationValue) / MaxValue;
					newTime = percentageDifference * AnimationTime;
					animationTimer = 0f;
				}
			}

			if (TimeType == BarTimeType.SecondsPerFillPercentage) {
				animationTime = newTime;
			}
		}

		private void AnimateFillAmount_NotAnimating(float newValue) {
			var positiveChange = newValue >= value;

			if (positiveChange) {
				if (UseSecondaryFillImage) SecondaryFillImage.fillAmount = newValue / MaxValue;
				MainFillImage.color = PositiveChangeColorMain;
				if (UseSecondaryFillImage) SecondaryFillImage.color = PositiveChangeColorSecondary;
			}
			else {
				MainFillImage.fillAmount = newValue / MaxValue;
				MainFillImage.color = NegativeChangeColorMain;
				if (UseSecondaryFillImage) SecondaryFillImage.color = NegativeChangeColorSecondary;
			}

			if (UseText) Text.text = FormatText(newValue);

			if (TimeType == BarTimeType.SecondsTotal) animationTime = AnimationTime;
			else if (TimeType == BarTimeType.SecondsPerFillPercentage) {
				var percentages = 100f * Mathf.Abs(newValue - value) / MaxValue;
				animationTime = percentages * AnimationTime;
			}

			animationTimer = 0f;
			animationStart = value;
			animationTarget = newValue;
			isAnimating = true;
			isAnimatingPositive = positiveChange;
		}

		private void SetFillAmount(float newValue) {
			MainFillImage.fillAmount = newValue / MaxValue;
			if (UseSecondaryFillImage) SecondaryFillImage.fillAmount = newValue / MaxValue;
			if (UseText) Text.text = FormatText(newValue);
		}

#region Public Methods

		public void SetValue(float newValue) {
			if (newValue <= 0) OnBarEmpty?.Invoke();
			if (newValue >= MaxValue) OnBarFull?.Invoke();
			newValue = newValue.Clamped(0, MaxValue);

			OnValueChangedEvent?.Invoke(value, newValue);
			AnimateFillAmount(newValue);
			value = newValue;
		}

		public void SetValueWithoutNotify(float newValue, bool animate = false) {
			newValue = newValue.Clamped(0, MaxValue);
			if (animate)
				AnimateFillAmount(newValue);
			else
				SetFillAmount(newValue);

			value = newValue;
		}

		public void ChangeValue(float delta) {
			var newValue = value + delta;
			SetValue(newValue);
		}

		public void ChangeValueWithoutNotify(float delta, bool animate = false) {
			var newValue = value + delta;
			SetValueWithoutNotify(newValue, animate);
		}

#endregion

#region Private Utilities

		[Validator]
		private bool ValidateFormat() {
			if (!TextFormat.Contains("%1")) return false;
			return true;
		}

		private string FormatText(float currentValue) {
			var text = TextFormat;
			var currentReg = @"%1(\:(0|[1-9]+[0-9]*))?";
			var maxReg = @"%2(\:(0|[1-9]+[0-9]*))?";

			var digitsCurrent = new List<int>();
			var digitsMax = new List<int>();
			var regex = new Regex(currentReg);
			var matches = regex.Matches(text);
			for (var i = 0; i < matches.Count; i++) {
				var match = matches[i];
				if (match.Groups.Count >= 2) {
					digitsCurrent.Add(int.Parse(match.Groups[2].Value));
				}

				text = regex.Replace(text, $"@1,{i}@", 1);
			}

			var regexMax = new Regex(maxReg);
			var matchesMax = regexMax.Matches(text);
			for (var i = 0; i < matchesMax.Count; i++) {
				var match = matchesMax[i];
				if (match.Groups.Count >= 2) {
					digitsMax.Add(int.Parse(match.Groups[2].Value));
				}

				text = regexMax.Replace(text, $"@2,{i}@", 1);
			}

			var currentFormatted = digitsCurrent.Select(digits => currentValue.ToString($"F{digits}")).ToList();
			for (var i = 0; i < currentFormatted.Count; i++) {
				text = text.Replace($"@1,{i}@", currentFormatted[i]);
			}

			var maxFormatted = digitsMax.Select(digits => MaxValue.ToString($"F{digits}")).ToList();
			for (var i = 0; i < maxFormatted.Count; i++) {
				text = text.Replace($"@2,{i}@", maxFormatted[i]);
			}
			return text;
		}

#endregion
	}

	public enum BarTimeType {
		[InspectorName("Seconds - Total")] 
		SecondsTotal,
		[InspectorName("Seconds - Per Fill Percentage")]
		SecondsPerFillPercentage
	}
}