/*
*   MoveNet
*   Copyright (c) 2021 Yusuf Olokoba.
*/

using System.Linq;

namespace NatSuite.ML.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Vision;

    /// <summary>
    /// MoveNet body pose visualizer.
    /// This visualizer uses visualizes the pose keypoints using a UI image.
    /// </summary>
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public sealed class BodyPoseVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// Render a body pose.
        /// </summary>
        /// <param name="image">Image which body pose is generated from.</param>
        /// <param name="pose">Body pose to render.</param>
        /// <param name="confidenceThreshold">Keypoints with confidence lower than this value are not rendered.</param>
        public void Render (Texture image, MoveNetPredictor.Pose pose, float confidenceThreshold = 0f) {
            // Delete current
            foreach (var point in currentPoints)
                GameObject.Destroy(point.gameObject);
            currentPoints.Clear();
            // Display image
            var imageTransform = transform as RectTransform;
            var rawImage = GetComponent<RawImage>();
            var aspectFitter = GetComponent<AspectRatioFitter>();
            rawImage.texture = image;
            aspectFitter.aspectRatio = (float)image.width / image.height;
            // Check
            if (pose == null)
                return;
            // Render keypoints
            
            // leftElbow  --> [7];
            // rightElbow --> [8];
            // leftWrist  --> [9];
            // rightWrist --> [10];

            // Calculate dist. between elbow and wrist, divide it by 3 and set the point at location wrist point + 1/3rd the distance
            
            // 1/3rd vector in the direction from left elbow to left wrist
            var leftPalmDotDirection = (pose[9] - pose[7]) / 3; 
            var leftPalmLocation = pose[9] + leftPalmDotDirection;
            
            // 1/3rd vector in the direction from right elbow to right wrist
            var rightPalmDotDirection = (pose[10] - pose[8]) / 3; 
            var rightPalmLocation = pose[10] + rightPalmDotDirection;
            
            Vector3[] palms = { leftPalmLocation, rightPalmLocation };

            for (int i = 0; i < palms.Length; i++)
            {
                // Check confidence
                if (palms[i].z < confidenceThreshold)
                {
                    return;
                }
                
                var anchor = Instantiate(keypoint, transform);
                anchor.gameObject.SetActive(false);
            
                // Position
                anchor.anchorMin = 0.5f * Vector2.one;
                anchor.anchorMax = 0.5f * Vector2.one;
                anchor.pivot = 0.5f * Vector2.one;
                anchor.anchoredPosition = Rect.NormalizedToPoint(imageTransform.rect, palms[i]);
                
                anchor.gameObject.SetActive(true);
                
                // Add
                currentPoints.Add(anchor);
            }
        }
        #endregion


        #region --Operations--
        [SerializeField] RectTransform keypoint;
        List<RectTransform> currentPoints = new List<RectTransform>();
        #endregion
    }
}