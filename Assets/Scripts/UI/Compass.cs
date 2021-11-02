using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] private RawImage compassImage;
    [SerializeField] private GameObject questIconPrefab;
    [SerializeField] private float maxDistance = 200f;

    public List<QuestMarker> questMarkers = new List<QuestMarker>();

    float compassUnit;

    //delete these
    public QuestMarker one, two, three;

    private void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width /360f;

        AddQuestMarker(one);
        AddQuestMarker(two);
        AddQuestMarker(three);
    }

    // This update is called once per frame in the InGameCanvasManager
    public void UpdateCompass(Transform camera)
    {
        compassImage.uvRect = new Rect(camera.localEulerAngles.y / 360f, 0f, 1f, 1f);
        
        foreach(QuestMarker marker in questMarkers)
        {
            marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker, camera);

            float distToMarker = Vector2.Distance(new Vector2(camera.position.x, camera.position.z), marker.Position);
            float scale = 0f;

            if(distToMarker < maxDistance)
            {
                scale = 1f - (distToMarker / maxDistance);
            }

            marker.image.rectTransform.localScale = Vector3.one *scale;

        }
    }

    public void AddQuestMarker(QuestMarker marker)
    {
        GameObject newMarker = Instantiate(questIconPrefab, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        questMarkers.Add(marker);
    }

    public void RemoveQuestMarker(QuestMarker marker)
    {
        if (questMarkers.Find(x => x == marker))
        {
            marker.image.sprite = null;
            questMarkers.Remove(marker);
        }
        else
            Debug.LogWarning("Questmarker does not exist and cannot be removed!");
    }

    Vector2 GetPosOnCompass (QuestMarker marker, Transform camera)
    {
        Vector2 playerPos = new Vector2(camera.position.x, camera.position.z);
        Vector2 playerFwd = new Vector2(camera.forward.x, camera.forward.z);

        float angle = Vector2.SignedAngle(marker.Position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}
