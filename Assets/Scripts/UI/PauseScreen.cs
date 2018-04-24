using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public Rect getTruncatedInnerRect(int width, int height)
    {
        int cx = Screen.width / 2;
        int cy = Screen.height / 2;

        return new Rect(cx - width / 2, cy - height / 2, width, height);
    }

    private int SelectedMenuEntry = 0;
    public bool IsShow;
    public Sprite backgroundSprite;
    private static readonly Color Grey = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    private static readonly Color LightGrey = new Color(0.6f, 0.6f, 0.6f, 0.5f);
    private static readonly Color Brown = new Color(0.42f, 0.21f, 0.13f);

    public float MoveDelayRemaining;
    public float MoveDelay = 0.1f;

    private void Update()
    {
        if (MoveDelayRemaining < 0)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                IsShow = !IsShow;
                MoveDelayRemaining = MoveDelay;
            }

            if (IsShow)
            {
                if (Input.GetButtonDown("Submit"))
                {
                    switch (SelectedMenuEntry)
                    {
                        case 0:
                            Hide();
                            MoveDelayRemaining = MoveDelay;
                            break;
                        case 1:
                            Exit();
                            break;
                    }
                }

                if (Input.GetAxis("Vertical") > 0)
                {
                    SelectedMenuEntry = Mathf.Max(SelectedMenuEntry - 1, 0);
                    MoveDelayRemaining = MoveDelay;
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    SelectedMenuEntry = Mathf.Min(SelectedMenuEntry + 1, 1);
                    MoveDelayRemaining = MoveDelay;
                }
            }
        }

        MoveDelayRemaining -= Time.deltaTime;
    }

    private void OnGUI()
    {
        if (IsShow)
        {
            DrawBackground();
            DrawMenu();
        }
    }

    public void Hide()
    {
        IsShow = false;
    }

    public void Show()
    {
        IsShow = true;
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void DrawBackground()
    {
        Rect rect = getTruncatedInnerRect(160, 200);
        IMUIHelper.DrawFilledRect(rect, Brown);
        if (backgroundSprite != null)
        {
            GUI.DrawTexture(rect, backgroundSprite.texture, ScaleMode.ScaleAndCrop);
        }
    }

    private void DrawMenu()
    {
        Rect rect = getTruncatedInnerRect(150, 190);
        int dy = 25;

        Rect upperRect = new Rect(rect.left, rect.top + dy, rect.width, dy);
        Rect lowerRect = new Rect(rect.left, rect.top + 3 * dy, rect.width, dy);
        Rect lowerRect2 = new Rect(rect.left, rect.top + 5 * dy, rect.width, dy);

        IMUIHelper.DrawFilledRect(upperRect, SelectedMenuEntry == 0 ? LightGrey : Grey);
        IMUIHelper.DrawFilledRect(lowerRect, SelectedMenuEntry == 1 ? LightGrey : Grey);
        GUI.Label(upperRect, "Back to Game");
        GUI.Label(lowerRect, "Main Menu");
        GUI.Label(lowerRect2, "Press Enter to Start");
        
    }
}