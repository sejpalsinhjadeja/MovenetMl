/* 
*   MoveNet
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using System.Runtime.InteropServices;
    using Internal;
    using Types;

    /// <summary>
    /// MoveNet single body pose predictor.
    /// </summary>
    public sealed partial class MoveNetPredictor : IMLPredictor<MoveNetPredictor.Pose> {

        #region --Client API--
        /// <summary>
        /// Create the MoveNet predictor.
        /// </summary>
        /// <param name="model">MoveNet ML model.</param>
        /// <param name="smoothing">Apply smoothing filter to detected points.</param>
        public MoveNetPredictor (MLModel model, bool smoothing = true) {
            this.model = model;
            this.filter = smoothing ? new OneEuroFilter(0.5f, 3f, 1f) : null;
        }

        /// <summary>
        /// Detect the body pose in an image.
        /// </summary>
        /// <param name="inputs">Input image.</param>
        /// <returns>Detected body pose.</returns>
        public unsafe Pose Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"MoveNet predictor expects a single feature", nameof(inputs));
            // Check type
            var input = inputs[0];
            if (!(input.type is MLArrayType type))
                throw new ArgumentException(@"MoveNet predictor expects an an array or image feature", nameof(inputs));        
            // Predict
            var inputType = model.inputs[0];
            var inputFeature = (input as IMLFeature).Create(inputType);
            var outputFeature = model.Predict(inputFeature)[0];
            inputFeature.ReleaseFeature();
            // Marshal
            var data = new float[17 * 3];
            Marshal.Copy(outputFeature.FeatureData(), data, 0, data.Length);
            outputFeature.ReleaseFeature();
            // Return
            data = filter != null ? filter.Filter(data) : data;
            var pose = new Pose(data);
            return pose;
        }
        #endregion


        #region --Operations--
        private readonly IMLModel model;
        private readonly OneEuroFilter filter;

        void IDisposable.Dispose () { } // Not used
        #endregion
    }
}