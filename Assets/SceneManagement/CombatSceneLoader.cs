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
    
    public async UniTask LoadCombatWith(EnemySetupData enemy, CancellationToken token)
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
        print("Loaded battle scene");
        _isBusy = false;
    }
    
    [Space]
    
    [SerializeField] private List<EnemySetupData> enemies;

    private int index = 0;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            LoadCombatWith(enemies[index], destroyCancellationToken).Forget();
            index = (index + 1) % enemies.Count;
        }
    }
}
