using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using LouisAshton.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatSceneLoader : PersistentSingleton<CombatSceneLoader>
{
    [SerializeField] private CombatContext context;
    [SerializeField] private SceneReference scene; 
    
    private bool _isBusy = false;
    
    public async UniTask LoadCombatWith(EnemySetupData enemy, Vector3 position = default, Quaternion rotation = default)
    {
        if (_isBusy)
            return;
        
        _isBusy = true;
        
        var currentBattleScene = SceneManager.GetSceneByBuildIndex(scene.BuildIndex);
        if (currentBattleScene.isLoaded) {
            await SceneManager.UnloadSceneAsync(currentBattleScene);
            print("Unloaded current battle scene");
        }
        
        context.EnemyData = enemy;
        
        await SceneManager.LoadSceneAsync(scene.BuildIndex, LoadSceneMode.Additive);
        
        var baseObject = FindAnyObjectByType<CardGameBaseObject>();
        if (baseObject) {
            baseObject.transform.position = position;
            baseObject.transform.rotation = rotation;
        }
        
        print("Loaded battle scene");
        _isBusy = false;
    }

    public async UniTask UnloadCombat()
    {
        if (_isBusy)
            return;
        
        _isBusy = true;
        
        var currentBattleScene = SceneManager.GetSceneByBuildIndex(scene.BuildIndex);
        if (currentBattleScene.isLoaded) {
            await SceneManager.UnloadSceneAsync(currentBattleScene);
            print("Unloaded current battle scene");
        }
        
        _isBusy = false;
    }
    
    // [Space]
    //
    // [SerializeField] private List<EnemySetupData> enemies;
    //
    // private int index = 0;
    //
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E)) {
    //         LoadCombatWith(enemies[index]).Forget();
    //         index = (index + 1) % enemies.Count;
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.Q)) {
    //         UnloadCombat().Forget();
    //     }
    // }
}
