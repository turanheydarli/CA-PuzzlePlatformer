using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Code.Scripts.Managers
{
    public class CameraManager : DestroyableManager<CameraManager>
    {
        [SerializeField] public List<CameraDictionary> virtualCameras;
        [SerializeField] string defaultCamera;

        void Start()
        {
            foreach (Transform cameraTransform in transform)
            {
                if (cameraTransform.TryGetComponent(out ICinemachineCamera virtualCamera))
                    virtualCameras.Add(new CameraDictionary
                    {
                        Key = cameraTransform.name,
                        Value = virtualCamera
                    });
            }
        }

        public void OpenCamera(string cameraName)
        {
            foreach (var virtualCamera in virtualCameras)
            {
                virtualCamera.Value.Priority = virtualCamera.Key == cameraName ? 11 : 10;
            }
        }

        public void Discover(string cameraName)
        {
            foreach (var virtualCamera in virtualCameras)
            {
                virtualCamera.Value.Priority = virtualCamera.Key == cameraName ? 11 : 10;
            }

            StartCoroutine(ReturnToDefault());
        }

        public void SetFollow(string cameraName, Transform objectTransform)
        {
            virtualCameras.FirstOrDefault(x => x.Key == cameraName)!.Value.Follow = objectTransform;
        }

        public void SetLookAt(string cameraName, Transform objectTransform)
        {
            virtualCameras.FirstOrDefault(x => x.Key == cameraName)!.Value.LookAt = objectTransform;
        }

        private IEnumerator ReturnToDefault()
        {
            yield return new WaitForSeconds(3);
            OpenCamera(defaultCamera);
        }
    }

    [Serializable]
    public class CameraDictionary
    {
        public string Key { get; set; }
        public ICinemachineCamera Value { get; set; }
    }
}