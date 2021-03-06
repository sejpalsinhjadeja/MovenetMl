namespace NatSuite.Examples
{
    using UnityEngine;
    using NatSuite.ML;
    using NatSuite.ML.Features;
    using NatSuite.ML.Vision;
    using NatSuite.ML.Visualizers;
    using UnityEngine.UI;
    using System.Linq;

    public class PoseDetectorHelper : MonoBehaviour
    {

        [Header(@"NatML Hub")]
        public string accessKey;

        [Header(@"Tracking")]
        public bool filter;

        [Header(@"UI")]
        public BodyPoseVisualizer visualizer;

        MLModelData modelData;
        MLModel model;
        MoveNetPredictor predictor;

        //  Down Added by sejpal
        public RawImage image;
        public RectTransform imageParent;
        public AspectRatioFitter imageFitter;

        // Device cameras
        WebCamDevice activeCameraDevice;
        WebCamTexture activeCameraTexture;

        public float threshhold;
        
        // Up Added by sejpal ==========

        async void Start()
        {
            Debug.Log("Fetching model data from NatML Hub");
            modelData = await MLModelData.FromHub("@natsuite/movenet", accessKey);
            model = modelData.Deserialize();
            predictor = new MoveNetPredictor(model, filter);
            if (WebCamTexture.devices.Length == 0) { return; }
            activeCameraDevice = WebCamTexture.devices.Last();
            activeCameraTexture = new WebCamTexture(activeCameraDevice.name);
            activeCameraTexture.filterMode = FilterMode.Trilinear;
            SetActiveCamera(activeCameraTexture);
        }

        public void SetActiveCamera(WebCamTexture cameraToUse)
        {
            if (activeCameraTexture != null) { activeCameraTexture.Stop(); }
            activeCameraTexture = cameraToUse;
            activeCameraDevice = WebCamTexture.devices.FirstOrDefault(device => device.name == cameraToUse.deviceName);
            image.texture = activeCameraTexture;
            image.material.mainTexture = activeCameraTexture;
            activeCameraTexture.Play();
        }

        void Update()
        {
            if (predictor == null)  return;
            if (!image.texture) return;
            var input = new MLImageFeature(activeCameraTexture.GetPixels32(), image.texture.width, image.texture.height);
            (input.mean, input.std) = modelData.normalization;
            var pose = predictor.Predict(input);
            visualizer.Render(image.texture, pose, threshhold);
        }

        void OnDisable()
        {
            model?.Dispose();
        }
    }
}