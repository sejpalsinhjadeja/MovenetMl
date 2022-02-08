/* 
*   MoveNet
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.Examples {

    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Video;
    using NatSuite.ML;
    using NatSuite.ML.Features;
    using NatSuite.ML.Vision;
    using NatSuite.ML.Visualizers;

    public class MoveNetSample : MonoBehaviour {

        [Header(@"NatML Hub")]
        public string accessKey;

        [Header(@"Tracking")]
        public VideoPlayer videoPlayer;
        public bool filter;

        [Header(@"UI")]
        public BodyPoseVisualizer visualizer;

        MLModelData modelData;
        MLModel model;
        MoveNetPredictor predictor;
        Texture2D frameTexture;

        async void Start () {
            // Create MoveNet predictor
            Debug.Log("Fetching model data from NatML Hub");
            modelData = await MLModelData.FromHub("@natsuite/movenet", accessKey);
            model = modelData.Deserialize();
            predictor = new MoveNetPredictor(model, filter);
            // Wait for video to start
            while (!videoPlayer.isPlaying)
                await Task.Yield();
            // Create frame texture
            frameTexture = new Texture2D((int)videoPlayer.width, (int)videoPlayer.height, TextureFormat.RGBA32, false);
            // Display video
            visualizer.Render(videoPlayer.texture, null);
        }

        void Update () {
            // Check that predictor is creared
            if (predictor == null)
                return;
            // Check that frame texture is created
            if (!frameTexture)
                return;
            // Readback
            ReadbackFrame(videoPlayer.texture, frameTexture);
            // Predict
            var input = new MLImageFeature(frameTexture);
            (input.mean, input.std) = modelData.normalization;
            var pose = predictor.Predict(input);
            // Visualize
            visualizer.Render(videoPlayer.texture, pose);
        }

        void OnDisable () {
            // Dispose model
            model?.Dispose();
        }

        static void ReadbackFrame (Texture src, Texture2D dst) {
            // Blit
            var tempRT = RenderTexture.GetTemporary(dst.width, dst.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(src, tempRT);
            // Readback
            var currentRT = RenderTexture.active;
            RenderTexture.active = tempRT;
            dst.ReadPixels(new Rect(0, 0, dst.width, dst.height), 0, 0);
            // Release
            RenderTexture.active = currentRT;
            RenderTexture.ReleaseTemporary(tempRT);
        }
    }
}