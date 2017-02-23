using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;

public class EnemyTool : EditorWindow
{
    string Myname = "";
    Sprite mySprite = null;
    int health;
    int attack;
    int defense;
    int agility;
    float attackTime;
    bool manaUser;
    int mana;

    bool nameFlag = false;
    bool spriteFlag = false;
    int lastChoice;
 

    List<Enemies> myEnemyList = new List<Enemies>();
    List<string> myEnemyNameList = new List<string>();
    string[] myEnemyNameArray = { "" };
    int myChoice = 0;
    [MenuItem("Custom tools/ Enemy Tool %g")]
    private static void Tool()
    {
        EditorWindow.GetWindow<EnemyTool>();
    }

    void Awake()
    {
        getEnemy();
    }

    private void OnGUI()
    {
        myChoice = EditorGUILayout.Popup(myChoice, myEnemyNameArray);

        Myname = EditorGUILayout.TextField("Name", Myname);
        if (nameFlag == true)
        {
            EditorGUILayout.HelpBox("The name can't be blank", MessageType.Error);
        }
        mySprite = (Sprite)EditorGUILayout.ObjectField("Sprite", mySprite, typeof(Sprite), allowSceneObjects: true);

        if (mySprite != null)
        {
            //Grab the sprite and show it
            Texture2D aTexture = SpriteUtility.GetSpriteTexture(mySprite, false);
            GUILayout.Label(aTexture);
        }
        if (spriteFlag == true)
        {
            EditorGUILayout.HelpBox("The sprite can't be blank", MessageType.Error);
        }

        health = EditorGUILayout.IntSlider("Health", health, 1, 300);
        attack = EditorGUILayout.IntSlider("Attack", attack, 1, 100);
        defense = EditorGUILayout.IntSlider("Defense", defense, 1, 100);
        agility = EditorGUILayout.IntSlider("Agility", agility, 1, 100);

        attackTime = EditorGUILayout.Slider("Attack time", attackTime, 1f, 20f);

        manaUser = EditorGUILayout.BeginToggleGroup("Magic User", manaUser);
        mana = EditorGUILayout.IntSlider("Mana", mana, 0, 100);
        EditorGUILayout.EndToggleGroup();

        if (myChoice == 0)
        {
            if (GUILayout.Button("Create"))
            {
                Create();
            }
        }
        else
        {
            if (GUILayout.Button("Save"))
            {
                alterEnemy();
            }
        }
        if (myChoice != lastChoice)
        {
            if (myChoice == 0)
            {
                //blank out fields for new enemy
                Myname = "";
                health = 1;
                attack = 1;
                defense = 1;
                agility = 1;
                attackTime = 1.0f;
                manaUser = false;
                mana = 0;
                mySprite = null;
            }
            else
            {
                LoadEmeny();
            }
            lastChoice = myChoice;
        }
    }

    private void getEnemy()
    {
        myEnemyList.Clear();
        myEnemyNameList.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Enemies");
        foreach (string guid in guids)
        {
            string s = AssetDatabase.GUIDToAssetPath(guid);
            Enemies e = AssetDatabase.LoadAssetAtPath<Enemies>(s) as Enemies;
            myEnemyList.Add(e);
        }
        foreach(Enemies e in myEnemyList)
        {
            myEnemyNameList.Add(e.emname);
            Debug.Log(e.emname);
        }

        myEnemyNameList.Insert(0, "New");
        myEnemyNameArray = myEnemyNameList.ToArray();

    }

    private void Create()
    {
        //checks to make sure the name and sprite are filled in
        if (Myname == "" || mySprite == null)
        {
            if (Myname == "" )
            {
                nameFlag = true;
            }
            else
            {
                nameFlag = false;
            }

            if (mySprite == null)
            {
                spriteFlag = true;
            }
            else
            {
                spriteFlag = false;
            }
            return;
        }
        else
        {
            //creates new enemy
            Enemies myEnemy = ScriptableObject.CreateInstance<Enemies>();
            myEnemy.emname = Myname;
            myEnemy.health = health;
            myEnemy.atk = attack;
            myEnemy.def = defense;
            myEnemy.agi = agility;
            myEnemy.atkTime = attackTime;
            myEnemy.isMagic = manaUser;
            myEnemy.manaPool = mana;
            myEnemy.mySprite = mySprite;
            AssetDatabase.CreateAsset(myEnemy, "Assets/Resources/Data/EnemyData/" + myEnemy.emname.Replace(" ", "_") + ".asset");
            nameFlag = false;
            spriteFlag = false;
            getEnemy();

            Myname = "";
            health = 1;
            attack = 1;
            defense = 1;
            agility = 1;
            attackTime = 1.0f;
            manaUser = false;
            mana = 0;
            mySprite = null;
        }
    }

    private void alterEnemy()
    {
        //saves all the stats
        myEnemyList[myChoice - 1].emname = Myname;
        myEnemyList[myChoice - 1].health = health;
        myEnemyList[myChoice - 1].atk = attack;
        myEnemyList[myChoice - 1].def = defense;
        myEnemyList[myChoice - 1].agi = agility;
        myEnemyList[myChoice - 1].atkTime = attackTime;
        myEnemyList[myChoice - 1].isMagic = manaUser;
        myEnemyList[myChoice - 1].manaPool = mana;
        myEnemyList[myChoice - 1].mySprite = mySprite;

        AssetDatabase.SaveAssets();
        Debug.Log("Saved");

    }

    private void LoadEmeny()
    {
        //load all the enemy stats 
        Myname = myEnemyList[myChoice - 1].emname;
        health = myEnemyList[myChoice - 1].health;
        attack = myEnemyList[myChoice - 1].atk;
        defense = myEnemyList[myChoice - 1].def;
        agility = myEnemyList[myChoice - 1].agi;
        attackTime = myEnemyList[myChoice - 1].atkTime;
        manaUser = myEnemyList[myChoice - 1].isMagic;
        mana = myEnemyList[myChoice - 1].manaPool;
        mySprite = myEnemyList[myChoice - 1].mySprite;
    }



}
