# -*- coding: utf-8 -*-
from pathlib import Path

scene_path = Path(r"F:\workspace\Snake\Assets\Scenes\SampleScene.unity")
text = scene_path.read_text(encoding="utf-8")

start = text.find("--- !u!1 &793577112\n")
end = text.find("--- !u!1 &809254119\n")
if start < 0 or end < 0:
    raise SystemExit("Could not find placeholder Image block")
text = text[:start] + text[end:]

replacements = [
    (
        """  m_UiScaleMode: 0
  m_ReferencePixelsPerUnit: 100
  m_ScaleFactor: 1
  m_ReferenceResolution: {x: 800, y: 600}
  m_ScreenMatchMode: 0
  m_MatchWidthOrHeight: 0
""",
        """  m_UiScaleMode: 1
  m_ReferencePixelsPerUnit: 100
  m_ScaleFactor: 1
  m_ReferenceResolution: {x: 1080, y: 1920}
  m_ScreenMatchMode: 0
  m_MatchWidthOrHeight: 0.5
""",
    ),
    (
        """  m_Children:
  - {fileID: 793577113}
  m_Father: {fileID: 202993426}
""",
        """  m_Children:
  - {fileID: 1600000101}
  - {fileID: 1600000201}
  - {fileID: 1600000301}
  m_Father: {fileID: 202993426}
""",
    ),
    (
        """  m_Script: {fileID: 11500000, guid: 84f1401a527db4340ac3a1e0de5fc925, type: 3}
  m_Name: 
  m_EditorClassIdentifier: 
""",
        """  m_Script: {fileID: 11500000, guid: 84f1401a527db4340ac3a1e0de5fc925, type: 3}
  m_Name: 
  m_EditorClassIdentifier: 
  startView: {fileID: 1600000104}
  playingView: {fileID: 1600000202}
  gameOverView: {fileID: 1600000304}
""",
    ),
    (
        """  m_Script: {fileID: 11500000, guid: 817666627cf10a742890a8e69c2a4226, type: 3}
  m_Name: 
  m_EditorClassIdentifier: 
""",
        """  m_Script: {fileID: 11500000, guid: 817666627cf10a742890a8e69c2a4226, type: 3}
  m_Name: 
  m_EditorClassIdentifier: 
  snake: {fileID: 0}
  uiController: {fileID: 1459150052}
""",
    ),
]

for old, new in replacements:
    if old not in text:
        raise SystemExit(f"Pattern not found:\n{old[:80]}")
    text = text.replace(old, new, 1)

ui_yaml = Path(r"F:\workspace\Snake\_ui_scene_fragment.yaml").read_text(encoding="utf-8")
if not text.endswith("\n"):
    text += "\n"
text += ui_yaml
if not text.endswith("\n"):
    text += "\n"

scene_path.write_text(text, encoding="utf-8")
print("Scene patched")
