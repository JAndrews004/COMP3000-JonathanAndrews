using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonSelection : MonoBehaviour
{
    public Button closeButton;
    public RectTransform panelRect;
    public GameObject DungeonEntryPrefab;

    private List<Rect> placedRects = new List<Rect>();
    [SerializeField] RectTransform closeButtonRect;
    // Start is called before the first frame update
    void Start()
    {
        closeButtonRect = closeButton.GetComponent<RectTransform>();
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        int averageLevel = 0;
        foreach(PartyMember mem in GameManager.Instance.PartyMembers)
        {
            averageLevel += mem.level;
        }

        int count = UnityEngine.Random.Range(4, 8);

        Rect closeRect = GetWorldRectInPanelSpace(closeButtonRect);
        placedRects.Add(closeRect);

        for (int i = 0; i < count; i++)
        {
            DungeonData data = new DungeonData();
            data.Generate(averageLevel / 4);

            GameObject newPrefab = Instantiate(DungeonEntryPrefab, transform);
            newPrefab.GetComponent<DungeonBoardEntry>().Bind(data);

            RectTransform rect = newPrefab.GetComponent<RectTransform>();

            bool placed = TryPlaceWithoutOverlap(rect);

            if (!placed)
            {
                Destroy(newPrefab);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool TryPlaceWithoutOverlap(RectTransform rect)
    {
        int maxAttempts = 5;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        float width = rect.rect.width;
        float height = rect.rect.height;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = UnityEngine.Random.Range(
                panelRect.rect.xMin + width / 2,
                panelRect.rect.xMax - width / 2
            );

            float y = UnityEngine.Random.Range(
                panelRect.rect.yMin + height / 2,
                panelRect.rect.yMax - height / 2
            );

            Rect newRect = new Rect(
                x - width / 2,
                y - height / 2,
                width,
                height
            );

            bool overlaps = false;

            foreach (Rect r in placedRects)
            {
                if (newRect.Overlaps(r))
                {
                    overlaps = true;
                    break;
                }
            }
            if (!overlaps)
            {
                rect.anchoredPosition = new Vector2(x, y);
                placedRects.Add(newRect);
                rect.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10f, 10f));
                return true;
            }
        }

        return false;
    }
    Rect GetWorldRectInPanelSpace(RectTransform target)
    {
        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        for (int i = 0; i < 4; i++)
        {
            corners[i] = panelRect.InverseTransformPoint(corners[i]);
        }

        Vector2 min = corners[0];
        Vector2 max = corners[2];

        return new Rect(min, max - min);
    }
}
