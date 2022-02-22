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
        //-JrujccPdr-yboDFvK1RP

        [Header(@"Tracking")]
        public bool filter;

        [Header(@"UI")]
        public BodyPoseVisualizer visualizer;

        MLModelData modelData;
        MLModel model;
        MoveNetPredictor predictor;
        Texture2D frameTexture;

        //  Down Added by sejpal
        public RawImage image;
        public RectTransform imageParent;
        public AspectRatioFitter imageFitter;

        // Device cameras
        WebCamDevice frontCameraDevice;
        WebCamDevice backCameraDevice;
        WebCamDevice activeCameraDevice;

        WebCamTexture frontCameraTexture;
        WebCamTexture backCameraTexture;
        WebCamTexture activeCameraTexture;

        // Image rotation
        Vector3 rotationVector = new Vector3(0f, 0f, 0f);

        // Image uvRect
        Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
        Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

        // Image Parent's scale
        Vector3 defaultScale = new Vector3(1f, 1f, 1f);
        Vector3 fixedScale = new Vector3(-1f, 1f, 1f);

        // Up Added by sejpal ==========

        async void Start()
        {
            // Create MoveNet predictor
            Debug.Log("Fetching model data from NatML Hub");
            modelData = await MLModelData.FromHub("@natsuite/movenet", accessKey);
            model = modelData.Deserialize();
            predictor = new MoveNetPredictor(model, filter);

            if (WebCamTexture.devices.Length == 0)
            {
                Debug.Log("No devices cameras found");
                return;
            }

            // Get the device's cameras and create WebCamTextures with them
            frontCameraDevice = WebCamTexture.devices.Last();
            backCameraDevice = WebCamTexture.devices.First();

            frontCameraTexture = new WebCamTexture(frontCameraDevice.name);
            backCameraTexture = new WebCamTexture(backCameraDevice.name);

            // Set camera filter modes for a smoother looking image
            frontCameraTexture.filterMode = FilterMode.Trilinear;
            backCameraTexture.filterMode = FilterMode.Trilinear;


            // Set the camera to use by default
            SetActiveCamera(frontCameraTexture);

        }

        public void SetActiveCamera(WebCamTexture cameraToUse)
        {
            if (activeCameraTexture != null)
            {
                activeCameraTexture.Stop();
            }

            activeCameraTexture = cameraToUse;
            activeCameraDevice = WebCamTexture.devices.FirstOrDefault(device => device.name == cameraToUse.deviceName);

            image.texture = activeCameraTexture;
            image.material.mainTexture = activeCameraTexture;

            activeCameraTexture.Play();
        }

        void Update()
        {
            // Check that predictor is creared
            if (predictor == null)
                return;
            // Check that frame texture is created
            if (!image.texture)
                return;
            // Predict
            var input = new MLImageFeature(activeCameraTexture.GetPixels32(), image.texture.width, image.texture.height);
            (input.mean, input.std) = modelData.normalization;
            var pose = predictor.Predict(input);
            // Visualize
            visualizer.Render(image.texture, pose);
        }

        void OnDisable()
        {
            // Dispose model
            model?.Dispose();
        }
    }
}