using System.Linq;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.Android;

namespace VR {
	public class DevicePerformanceAndAuthorizationController : MonoBehaviour {
		protected void Start() {
			Application.targetFrameRate = 300;
			if (Performance.TryGetDisplayRefreshRate(out var rate)) {
				float newRate = 120f; // fallback to this value if the query fails.
				if (Performance.TryGetAvailableDisplayRefreshRates(out var rates)) {
					newRate = rates.Max();
				}

				if (rate < newRate) {
					if (Performance.TrySetDisplayRefreshRate(newRate)) {
						Time.fixedDeltaTime = 1f / newRate;
						Time.maximumDeltaTime = 1f / newRate;
					}
				}
			}

			OVRPlugin.foveatedRenderingLevel = OVRPlugin.FoveatedRenderingLevel.High;

			if (!Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
				// We do not have permission to use the microphone.
				// Ask for permission or proceed without the functionality enabled.
				Permission.RequestUserPermission(Permission.Microphone);
			}

			Debug.Log($"Set application target rate to 300 but the rate is {Application.targetFrameRate}");
			Debug.Log(
				$"User authorized microphone usage {Permission.HasUserAuthorizedPermission(Permission.Microphone)}");
		}
	}
}
