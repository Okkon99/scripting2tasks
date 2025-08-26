using UnityEditor;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        SceneView view = SceneView.lastActiveSceneView;
        Camera scene_view_camera = view.camera;
        Camera game_camera = Camera.allCameras[0];
        game_camera.transform.SetPositionAndRotation(scene_view_camera.transform.position, scene_view_camera.transform.rotation);
#endif
    }
}
