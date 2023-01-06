using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Code.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] public List<CameraDictionary> virtualCameras;

        public static CameraManager Instance { get; private set; }

        private void Awake()
        {
            Instance ??= this;
        }

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

        public void SetFollow(string cameraName, Transform objectTransform)
        {
            virtualCameras.FirstOrDefault(x => x.Key == cameraName)!.Value.Follow = objectTransform;
        }
        
        public void SetLookAt(string cameraName, Transform objectTransform)
        {
            virtualCameras.FirstOrDefault(x => x.Key == cameraName)!.Value.LookAt = objectTransform;
        }
    }

    [Serializable]
    public class CameraDictionary
    {
        public string Key { get; set; }
        public ICinemachineCamera Value { get; set; }
    }
}