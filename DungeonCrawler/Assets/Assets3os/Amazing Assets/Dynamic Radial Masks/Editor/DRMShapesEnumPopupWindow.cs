using UnityEngine;
using UnityEditor;


namespace AmazingAssets.DynamicRadialMasksEditor
{
    internal class DRMShapesEnumPopupWindow : PopupWindowContent
    {
        public delegate void Callback(int value);
        Callback callbakcMethod;


        Vector2 drawRectSize = new Vector2(68, 86); 
        int textureDrawSize = 64;
        int selecion = 0;


        public DRMShapesEnumPopupWindow(int _selecion, Callback _callbakcMethod)
        {
            this.selecion = _selecion;
            this.callbakcMethod = _callbakcMethod;
        }

        public override void OnGUI(Rect rect)
        {
            for (int i = 0; i < DRMEditorResources.shapeNames.Length; i++)
            {
                Rect drawRect = new Rect(i * drawRectSize.x, 2, drawRectSize.x, drawRectSize.y);                              

                if (selecion == i)
                    EditorGUI.DrawRect(drawRect, GUI.skin.settings.selectionColor);


                EditorGUI.LabelField(new Rect(drawRect.xMin, drawRect.yMin, drawRect.width, 18), DRMEditorResources.shapeNames[i], selecion == i ? DRMEditorResources.LabelMiniCenterBold : DRMEditorResources.LabelMiniCenter);


                drawRect = new Rect(drawRect.xMin + 2, drawRect.yMax - textureDrawSize - 4, textureDrawSize, textureDrawSize);
                GUI.DrawTexture(drawRect, DRMEditorResources.IconShapeTexture(i));
                if (drawRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    selecion = i;

                    if (callbakcMethod != null)
                        callbakcMethod(selecion);

                    if (Event.current.clickCount > 1)
                    {
                        editorWindow.Close();
                    }
                    else
                    {
                        editorWindow.Repaint();
                    }
                }
            }


            if(Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.LeftArrow)
                    selecion -= 1;

                if (Event.current.keyCode == KeyCode.RightArrow)
                    selecion += 1;

                if (Event.current.keyCode == KeyCode.Return)
                    editorWindow.Close();



                selecion = Mathf.Clamp(selecion, 0, DRMEditorResources.shapeNames.Length - 1);

                if (callbakcMethod != null)
                    callbakcMethod(selecion);

                editorWindow.Repaint();
            }
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(drawRectSize.x * 8, drawRectSize.y);
        }
    }
}