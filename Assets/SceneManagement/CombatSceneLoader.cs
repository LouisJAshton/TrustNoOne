using System.Collections.Generic;
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
        
        await Unload();

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

    private async UniTask Unload()
    {
        var currentBattleScene = SceneManager.GetSceneByBuildIndex(scene.BuildIndex);

        var lerpCam = FindAnyObjectByType<CameraLerp>();
        if (lerpCam && lerpCam.isActiveAndEnabled) {
            await lerpCam.MoveBack(Application.exitCancellationToken);
        }
        
        if (currentBattleScene.isLoaded) {
            await SceneManager.UnloadSceneAsync(currentBattleScene);
            print("Unloaded current battle scene");
        }
    }

    public async UniTask UnloadCombat()
    {
        if (_isBusy)
            return;
        
        _isBusy = true;

        await Unload();
        
        _isBusy = false;
    }
    
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.E)) {
        //     LoadCombatWith(enemies[index]).Forget();
        //     index = (index + 1) % enemies.Count;
        // }
    
        if (Input.GetKeyDown(KeyCode.Q)) {
            UnloadCombat().Forget();
        }
    }
}
