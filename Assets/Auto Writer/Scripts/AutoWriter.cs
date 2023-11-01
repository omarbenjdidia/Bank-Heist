using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class AutoWriter : MonoBehaviour
{
	
	public bool TextActive = true;
	 bool IsRunning;
	
	
	public TextMesh TextMeshLink;
	public TextAsset MyTextFile;
	public List< AudioSource> TypeSound;
	public List< AudioSource> SpecialTypeSound;
	 
	public float StartDelay = 0;
	public bool StartDelayGlobal = false;
	public float TypeInterval = 0.2f;
	public float TypeIntervalVariant = 0.05f;
	public bool TypeIntervalGLobal = false;
	public bool LoopTextRead = false;
	public bool LoopClear = false;
	public bool ClearTextMeshStringAtStart = true;
	
	public bool SoundOnEveryChar = false;
	public bool UseSpecialSound = false;
	public string NoSound = "\n "; 
	public string SpecialSound = "\n";
	
	public bool TypeWholeWords = false;
	public string NotWordChar = "\n ";

	string LoadedText;
	int TextLength = 0;
	int CurrentIndex = 0;
	
	float StartDelayTimer = 0;
	float IntervalTimer = 0;
	
	bool SoundHasBeenMadeThisFrame = false;
	
	public bool ChainNext = false;
	public AutoWriter NextChain;
	public bool ResetChainAtStart = true;
	public bool TurnSelfOffIfCallChain = true;
	

	// Use this for initialization
	void Start ()
	{
		//make sure the active update works the first time
		IsRunning = !TextActive;
		
		if(MyTextFile != null)
		{
			LoadedText = MyTextFile.text;
			TextLength = LoadedText.Length;
		}
		else
		{
			PrintError(2);
		}
		
		
		
		if(ClearTextMeshStringAtStart)
		{
			if( TextMeshLink != null)
			{
				//clear the string 
				TextMeshLink.text = "";
			}
			else
			{
				PrintError(3);
			}
		}
		
		if(StartDelay <= 0)
		{
			StartDelayTimer = 1;
		}
		
		if(ChainNext)
		{			
			if(NextChain == null)
			{
				PrintError(4);
			}
			else
			{
				NextChain.TurnOffForChain();
			}
		}
			
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(UpdateTextActive())
		{
			if(StartDelayTimer > StartDelay)
			{
				TypeUpdate();
			}
			else
			{
				StartDelayTimer = UpdateTimer(StartDelayTimer,StartDelayGlobal);
			}
		}
	
	}
	
	//main Update
	void TypeUpdate()
	{
		if(LoopTextRead || CurrentIndex < TextLength)
		{
			IntervalTimer = UpdateTimer(IntervalTimer,TypeIntervalGLobal);
			SoundHasBeenMadeThisFrame = false;
			
			//loop in case we should type more than one letter this frame
			while(IntervalTimer > TypeInterval)
			{
				IntervalTimer -= TypeInterval;
				
				if(TypeWholeWords)
				{
					PrintOutWord();
				}
				else
				{
					PrintOutChar();
				}
			}			
		}
		else if(ChainNext) 
		{
			if(NextChain != null)
			{
				if(!NextChain.TextActive) // NextChain should ahve been disabled at start, we want to turn it on now
				{
					NextChain.TextActive = true;
					
					NextChain.ResetReadOut();
					
					TextActive = !TurnSelfOffIfCallChain;// we are finished disable, dont clear readout
				}
			}
			
		}
		
	}
	
	//Main update for whole word output
	void PrintOutWord()
	{		
		do 
		{
			if(CurrentIndex < TextLength)
			{				
				PlaySoundEffect();			
				TextMeshLink.text += LoadedText[CurrentIndex];
				CurrentIndex++;
			}
			else if(LoopTextRead)
			{
				if(LoopClear)
				{
					if(TextMeshLink != null)
					{
						TextMeshLink.text = "";
					}
				}
				CurrentIndex = 0;
				return;
			}
		}while(CurrentIndex < TextLength &&
			!CheckContains(NotWordChar,LoadedText[CurrentIndex]));
		
	}
	
	//Main Update for single char output
	void PrintOutChar()
	{
		if(CurrentIndex < TextLength)
		{					
			PlaySoundEffect();
			
			TextMeshLink.text += LoadedText[CurrentIndex];
			CurrentIndex++;
		}
		else if(LoopTextRead)
		{
				if(LoopClear)
				{
					if(TextMeshLink != null)
					{
						TextMeshLink.text = "";
					}
				}
			CurrentIndex = 0;
		}
	}
	
	// for use external use from animation or another script
	public void CompletePrintOut()
	{		
		SoundHasBeenMadeThisFrame = false;
		
		while(CurrentIndex < TextLength)
		{					
			PlaySoundEffect();
			
			TextMeshLink.text += LoadedText[CurrentIndex];
			CurrentIndex++;
		}
	
	}
	
	//clears the readout used for chain // can be used from animation safely 
	public void ResetReadOut()
	{			
		CurrentIndex = 0;		
		if(ResetChainAtStart)
		{
			TextMeshLink.text = "";		
		}
	
	}
	
	//called on a script that is linked in to be a chained type 
	public void TurnOffForChain()
	{
		if(TextActive)
		{
			TextActive = false;
		}
		
		if(ClearTextMeshStringAtStart)
		{
			TextMeshLink.text = "";
		}
	}
	
	//Create Typing Sound Effect
	void PlaySoundEffect()
	{
		if(!SoundHasBeenMadeThisFrame)
		{
			if(UseSpecialSound)
			{
				if(CheckContains(SpecialSound,LoadedText[CurrentIndex]))
				{
					if(SpecialTypeSound != null)
					{
						SoundHasBeenMadeThisFrame = true;
						if(SpecialTypeSound.Count == 1)
						{
							if(SpecialTypeSound[0] != null)
							{
								SpecialTypeSound[0].PlayOneShot(SpecialTypeSound[0].clip);
							}
							else
							{
								PrintError(0);
							}
						}
						else if(SpecialTypeSound.Count > 0)
						{
							int Index = Random.Range(0,TypeSound.Count-1);
							if(SpecialTypeSound[Index] != null)
							{
								SpecialTypeSound[Index].PlayOneShot(SpecialTypeSound[Index].clip);
							}
							else
							{
								PrintError(0);
							}
						}
					}
					else
					{
						PrintError(0);
					}
					return;
				}
				
			}
			if(SoundOnEveryChar &&
				!CheckContains(NoSound,LoadedText[CurrentIndex]))
			{
				if(TypeSound != null)
				{
					SoundHasBeenMadeThisFrame = true;
					if(TypeSound.Count == 1)
					{
						if(TypeSound[0] != null)
						{
							TypeSound[0].PlayOneShot(TypeSound[0].clip);
						}
						else
						{
							PrintError(1);
						}
					}
					else if(TypeSound.Count > 0)
					{
						int Index = Random.Range(0,TypeSound.Count-1);
						if(TypeSound[Index] != null)
						{
							TypeSound[Index].PlayOneShot(TypeSound[Index].clip);
						}
						else
						{
							PrintError(1);
						}
					}
				}
				else
				{
					PrintError(1);
				}
				return;
			}
		}
	}
	
	//Return the new Timer Value
	float UpdateTimer(float Timer,bool IsGlobal)
	{
		//if global is true then unscale the delta time
		if(!IsGlobal)
		{
			return Timer + Time.deltaTime;
		}
		else
		{
			//do not divide is we dont ahve to
			if(Time.timeScale != 1 )
			{
				return Timer + (Time.deltaTime/Time.timeScale);
			}
			else
			{
				return Timer + Time.deltaTime;
			}
		}
	}
	
	bool UpdateTextActive()
	{
			if(TextActive != IsRunning && TextMeshLink && TextMeshLink.GetComponent<Renderer>())
			{
				TextMeshLink.GetComponent<Renderer>().enabled = TextActive;
				IsRunning = TextActive;
			}
		return TextActive;
	}
	
	// returns true if the string contains the char
	bool CheckContains(string IsIn, char LookFor)
	{
		for(int i = 0; i < IsIn.Length; ++i)
		{
			if(LookFor == IsIn[i])
			{
				return true;
			}			
		}
		return false;
	}
	
	//Function For Errors, prionts to the console in editor
	void PrintError(int ErrorCode)
	{
		switch(ErrorCode)
		{
		case 0:
			print("Auto Writer ERROR CODE: " +ErrorCode+" " + gameObject.name +" Special Sound Not Found!");
			break;
		case 1:
			print("Auto Writer ERROR CODE: " +ErrorCode+" " + gameObject.name +" Type Sound Not Found!");
			break;
		case 2:
			print("Auto Writer ERROR CODE: " +ErrorCode+" " + gameObject.name +" Text File Not Found!");
			break;
		case 3:
			print("Auto Writer ERROR CODE: " +ErrorCode+" " + gameObject.name +" Text Mesh Not Found!");
			break;
		case 4:
			print("Auto Writer ERROR CODE: " +ErrorCode+" " + gameObject.name +" Chain Not Found!");
			break;
		}
	}
}
