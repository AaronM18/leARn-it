//-----------------------------------------------------------------------
// <copyright file="AugmentedImageExampleController.cs" company="Google">
//
// Copyright 2018 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controller for AugmentedImage example.
    /// </summary>
    /// <remarks>
    /// In this sample, we assume all images are static or moving slowly with
    /// a large occupation of the screen. If the target is actively moving,
    /// we recommend to check <see cref="AugmentedImage.TrackingMethod"/> and
    /// render only when the tracking method equals to
    /// <see cref="AugmentedImageTrackingMethod"/>.<c>FullTracking</c>.
    /// See details in <a href="https://developers.google.com/ar/develop/c/augmented-images/">
    /// Recognize and Augment Images</a>
    /// </remarks>
    public class AugmentedImageExampleController : MonoBehaviour
    {
        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;
        public List<AugmentedImageVisualizer> prefabs = new List<AugmentedImageVisualizer>();

        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;
        public GameObject logo;
        public GameObject menu;
        public GameObject about;
        public GameObject soon;
        public GameObject backButton;
        public bool scanMode;

        private Dictionary<int, AugmentedImageVisualizer> m_Visualizers = new Dictionary<int, AugmentedImageVisualizer>();

        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
            /* UI CONTROLLERS */
            logo.SetActive(true);
            menu.SetActive(true);
            backButton.SetActive(false);
            about.SetActive(false);
            soon.SetActive(false);
            scanMode = false;
        }

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do
            // not previously have a visualizer. Remove visualizers for stopped images.
            foreach (var image in m_TempAugmentedImages)
            {
                AugmentedImageVisualizer visualizer = null;

                m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);

                if (image.TrackingState == TrackingState.Tracking && visualizer == null && image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
                {
                    addVisualizerImage(image, visualizer);
                }
                else if (image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && visualizer  != null)
                {
                    m_Visualizers.Remove(image.DatabaseIndex);
                    GameObject.Destroy(visualizer.gameObject);
                }
            }

            // Show the fit-to-scan overlay if there are no images that are Tracking.
            foreach (var visualizer in m_Visualizers.Values)
            {
                if (visualizer.Image.TrackingState == TrackingState.Tracking)
                {
                    FitToScanOverlay.SetActive(false);
                    return;
                }
            }

            if (scanMode == true)
            {
                FitToScanOverlay.SetActive(true);
            }
        }

        public void addVisualizerImage(AugmentedImage image, AugmentedImageVisualizer visualizer)
        {
            // Create an anchor to ensure that ARCore keeps tracking this augmented image.
            switch (image.Name)
            {
                case "cat":
                    setAugmentedImage(image, visualizer, prefabs[0]);
                    break;
                case "duck":
                    setAugmentedImage(image, visualizer, prefabs[1]);
                    break;
                case "frog":
                    setAugmentedImage(image, visualizer, prefabs[2]);
                    break;
                case "horse":
                    setAugmentedImage(image, visualizer, prefabs[3]);
                    break;
                case "koala":
                    setAugmentedImage(image, visualizer, prefabs[4]);
                    break;
                case "monkey":
                    setAugmentedImage(image, visualizer, prefabs[5]);
                    break;
                case "parrot":
                    setAugmentedImage(image, visualizer, prefabs[6]);
                    break;
                case "seahorse":
                    setAugmentedImage(image, visualizer, prefabs[7]);
                    break;
                case "shark":
                    setAugmentedImage(image, visualizer, prefabs[8]);
                    break;
                case "turtle":
                    setAugmentedImage(image, visualizer, prefabs[9]);
                    break;
                case "deer":
                    setAugmentedImage(image, visualizer, prefabs[10]);
                    break;
                case "dog":
                    setAugmentedImage(image, visualizer, prefabs[11]);
                    break;
                case "fish":
                    setAugmentedImage(image, visualizer, prefabs[12]);
                    break;
                case "kangaroo":
                    setAugmentedImage(image, visualizer, prefabs[13]);
                    break;
                case "panda":
                    setAugmentedImage(image, visualizer, prefabs[14]);
                    break;
                case "sheep":
                    setAugmentedImage(image, visualizer, prefabs[15]);
                    break;
                default:
                    break;
            }
        }

        public void setAugmentedImage(AugmentedImage image, AugmentedImageVisualizer visualizer, AugmentedImageVisualizer prefab)
        {
            Anchor anchor = image.CreateAnchor(image.CenterPose);
            visualizer = (AugmentedImageVisualizer)Instantiate(prefab, anchor.transform);
            visualizer.Image = image;
            m_Visualizers.Add(image.DatabaseIndex, visualizer);
        }

        /* UI CONTROLLERS */
        public void goLearnPage()
        {
            scanMode = true;
            logo.SetActive(false);
            menu.SetActive(false);
            backButton.SetActive(true);
        }
        
        public void goBack()
        {
            scanMode = false;
            logo.SetActive(true);
            menu.SetActive(true);
            backButton.SetActive(false);
            about.SetActive(false);
            soon.SetActive(false);
            FitToScanOverlay.SetActive(false);
        }

        public void goAboutPage()
        {
            scanMode = false;
            menu.SetActive(false);
            about.SetActive(true);
            backButton.SetActive(true);
        }

        public void goCategoriesPage()
        {
            scanMode = false;
            menu.SetActive(false);
            soon.SetActive(true);
            backButton.SetActive(true);
        }
    }
}
