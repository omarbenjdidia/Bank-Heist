using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
[CustomEditor(typeof(AutoWriter))]
[CanEditMultipleObjects]
public class AutoWriterEditorScript : Editor
{
	
	GUIContent TempLable;
	
	
	public SerializedProperty eTextActive;
	
	
	public SerializedProperty eTextMeshLink;
	public SerializedProperty eMyTextFile;
	public SerializedProperty eTypeSound;
	public SerializedProperty eSpecialTypeSound;
	 
	public SerializedProperty eStartDelay ;
	public SerializedProperty eStartDelayGlobal ;
	public SerializedProperty eTypeInterval;
	public SerializedProperty eTypeIntervalVariant ;
	public SerializedProperty eTypeIntervalGLobal;
	public SerializedProperty eLoopTextRead;
	public SerializedProperty eLoopClear;
	public SerializedProperty eClearTextMeshStringAtStart;
	
	public SerializedProperty eSoundOnEveryChar;
	public SerializedProperty eUseSpecialSound;
	public SerializedProperty eNoSound; 
	public SerializedProperty eSpecialSound;
	
	public SerializedProperty eTypeWholeWords;
	public SerializedProperty eNotWordChar;
	
	public SerializedProperty eChainNext;
	public SerializedProperty eNextChain;
	public SerializedProperty eResetChainAtStart;
	public SerializedProperty eTurnSelfOffIfCallChain;
	
	
	public SerializedProperty eTypeSoundSize;
	public SerializedProperty eSpecialTypeSoundSize;
	public SerializedProperty eArrayIterator;
	
	
    public void OnEnable()
    {
		TempLable = new GUIContent();
		
		
		eTextActive = serializedObject.FindProperty("TextActive");
		
		eTextMeshLink = serializedObject.FindProperty("TextMeshLink");
		eMyTextFile = serializedObject.FindProperty("MyTextFile");
		eStartDelay = serializedObject.FindProperty("StartDelay");
		
		eStartDelayGlobal = serializedObject.FindProperty("StartDelayGlobal");
		eTypeInterval = serializedObject.FindProperty("TypeInterval");
		eTypeIntervalVariant = serializedObject.FindProperty("TypeIntervalVariant");
		eTypeIntervalGLobal = serializedObject.FindProperty("TypeIntervalGLobal");
		
		eLoopTextRead = serializedObject.FindProperty("LoopTextRead");
		eLoopClear = serializedObject.FindProperty("LoopClear");
		eClearTextMeshStringAtStart = serializedObject.FindProperty("ClearTextMeshStringAtStart");
		
		eSoundOnEveryChar = serializedObject.FindProperty("SoundOnEveryChar");
		eUseSpecialSound = serializedObject.FindProperty("UseSpecialSound");
		eNoSound = serializedObject.FindProperty("NoSound");
		eSpecialSound = serializedObject.FindProperty("SpecialSound");
		
		eTypeWholeWords = serializedObject.FindProperty("TypeWholeWords");
		eNotWordChar = serializedObject.FindProperty("NotWordChar");
		
		eChainNext = serializedObject.FindProperty("ChainNext");
		eNextChain = serializedObject.FindProperty("NextChain");
		eResetChainAtStart = serializedObject.FindProperty("ResetChainAtStart");
		eTurnSelfOffIfCallChain = serializedObject.FindProperty("TurnSelfOffIfCallChain");
		
		eTypeSound = serializedObject.FindProperty("TypeSound");
		eSpecialTypeSound = serializedObject.FindProperty("SpecialTypeSound");
		
		eTypeSoundSize = serializedObject.FindProperty("TypeSound.Array.size");
		eSpecialTypeSoundSize = serializedObject.FindProperty("SpecialTypeSound.Array.size");
		
    }

	
  public override void OnInspectorGUI () 
	{			
		serializedObject.Update();
				
		
		Display();
		
		
		serializedObject.ApplyModifiedProperties();
	}
	
	void Display()
	{				
		
		EditorGUILayout.Space();//////////////////////////////////////////////////////////////////////////////
		
		TempLable.text = "Text Active";
		EditorGUILayout.PropertyField( eTextActive,TempLable);
		
		
		EditorGUILayout.Space();///////////////////////////////////////////////////////////////////////////////////////////	
		EditorGUILayout.LabelField("TEXT LINK",EditorStyles.boldLabel);
					
		TempLable.text = "Text file";
		EditorGUILayout.PropertyField( eMyTextFile,TempLable);		
		
		TempLable.text = "Text Mesh";
		EditorGUILayout.PropertyField( eTextMeshLink,TempLable);
		
		
		EditorGUILayout.Space();	///////////////////////////////////////////////////////////////////////////////////////////		
		EditorGUILayout.LabelField("STARTUP",EditorStyles.boldLabel);			
		
		TempLable.text = "Clear Text on start";
		EditorGUILayout.PropertyField( eClearTextMeshStringAtStart,TempLable);
		
		//TempLable.text = "Text Active";
		EditorGUILayout.PropertyField( eStartDelay);
					
		TempLable.text = "Delay in real time";
		EditorGUILayout.PropertyField( eStartDelayGlobal,TempLable);
		
		
		EditorGUILayout.Space();	///////////////////////////////////////////////////////////////////////////////////////////				
		EditorGUILayout.LabelField("SOUND",EditorStyles.boldLabel);			
			
		TempLable.text = "Sound Effects";
		EditorGUILayout.PropertyField( eSoundOnEveryChar,TempLable);
			
		if(eSoundOnEveryChar.boolValue)
		{				
			TempLable.text = "Mute Characters";
			EditorGUILayout.PropertyField(eNoSound,TempLable);
			
			EditorGUILayout.PropertyField(eTypeSound);			
			if(eTypeSound.isExpanded)
			{
				EditorGUI.indentLevel = 2;
				int SizeBeforeChange = eTypeSoundSize.intValue;
				EditorGUILayout.PropertyField( eTypeSoundSize);
				if(SizeBeforeChange > eTypeSoundSize.intValue)
				{
					 SizeBeforeChange = eTypeSoundSize.intValue;
				}
				for(int i = 0; i < SizeBeforeChange; ++i)
				{
					eArrayIterator = serializedObject.FindProperty(("TypeSound.Array.data["+ i +"]"));
					EditorGUILayout.PropertyField( eArrayIterator);
				}
				
				EditorGUI.indentLevel = 0;
			}
				
		}
		
		TempLable.text = "Special Sound Effects";
		EditorGUILayout.PropertyField( eUseSpecialSound,TempLable);
			
		if(eUseSpecialSound.boolValue)
		{				
			TempLable.text = "Special Characters";
			EditorGUILayout.PropertyField(eSpecialSound, TempLable);
			
			TempLable.text = "Special Sounds";
			EditorGUILayout.PropertyField(eSpecialTypeSound,TempLable);			
			if(eSpecialTypeSound.isExpanded)
			{
				EditorGUI.indentLevel = 2;
				int SizeBeforeChange = eSpecialTypeSoundSize.intValue;
				EditorGUILayout.PropertyField( eSpecialTypeSoundSize);
				if(SizeBeforeChange > eSpecialTypeSoundSize.intValue)
				{
					 SizeBeforeChange = eSpecialTypeSoundSize.intValue;
				}
				for(int i = 0; i < SizeBeforeChange; ++i)
				{
					eArrayIterator = serializedObject.FindProperty(("SpecialTypeSound.Array.data["+ i +"]"));
					EditorGUILayout.PropertyField( eArrayIterator);
				}
				
				EditorGUI.indentLevel = 0;
			}
				
		}	
		
		EditorGUILayout.Space();///////////////////////////////////////////////////////////////////////////////////////////	
		EditorGUILayout.LabelField("DISPLAY",EditorStyles.boldLabel);		
		
		TempLable.text = "Repeat";
		EditorGUILayout.PropertyField( eLoopTextRead,TempLable);
		
		
		TempLable.text = "Loop";
		EditorGUILayout.PropertyField( eLoopClear,TempLable);
		
		
		TempLable.text = "Write Whole Words";
		EditorGUILayout.PropertyField( eTypeWholeWords,TempLable);
		
		if(eTypeWholeWords.boolValue)
		{			
			TempLable.text = "Non-Words";
			EditorGUILayout.PropertyField( eNotWordChar,TempLable);
		}
							
		
		EditorGUILayout.Space();	///////////////////////////////////////////////////////////////////////////////////////////		
		if(eTypeWholeWords.boolValue)
		{		
			EditorGUILayout.LabelField("WORD DELAY",EditorStyles.boldLabel);
		}
		else
		{			
			EditorGUILayout.LabelField("LETTER DELAY",EditorStyles.boldLabel);
		}		
		TempLable.text = "Real world time";
		EditorGUILayout.PropertyField( eTypeIntervalGLobal,TempLable);
		
		TempLable.text = "Interval";
		EditorGUILayout.PropertyField( eTypeInterval,TempLable);
	
		TempLable.text = "Varient";
		EditorGUILayout.PropertyField( eTypeIntervalVariant,TempLable);
		
			
		EditorGUILayout.Space();	///////////////////////////////////////////////////////////////////////////////////////////		
		EditorGUILayout.LabelField("CHAIN",EditorStyles.boldLabel);
					
		TempLable.text = "Trigger when done";
		EditorGUILayout.PropertyField( eChainNext,TempLable);
		
		if(eChainNext.boolValue)
		{					
			TempLable.text = "Trigger object";
			EditorGUILayout.PropertyField( eNextChain,TempLable);
			
			TempLable.text = "Reset trigger object";
			EditorGUILayout.PropertyField( eResetChainAtStart,TempLable);		
			
			TempLable.text = "Deactivate self";
			EditorGUILayout.PropertyField( eTurnSelfOffIfCallChain,TempLable);		
			
			
			
		}
		
	}
	
}