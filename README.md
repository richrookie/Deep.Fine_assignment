## Deep.Fine_Tech-Screening

### [유니티 버전]
- 2022.3.21f1

### [프로젝트 구성]
- **00Scenes** : 1_LoadAsset, 2_LoadAssetAsync, 3_LoadAssetAsync
- **01Scripts** : AssetLoader, AssetLoader2, AssetLoader3, LoaderModule
- **Resources** : Cube.mat, CubePrefab.prefab, ironman.obj

<br>

### [실행 방법]
### 1. .obj 로드

<img src="https://github.com/richrookie/Deep.Fine_assignment/assets/83854046/5f46dabf-bf72-4864-8508-9e49c30a0635" alt="Description" width="700" />

<br>

### 2. 렌더링 결과물 확인
1. 유니티 코루틴(Coroutine)<br>
유니티 코루틴(Coroutine)을 사용하고, 동적으로 데이터를 다운로드 하여 3d model 렌더링
<img src="https://github.com/richrookie/Deep.Fine_assignment/assets/83854046/0b5660a2-3c97-4241-b7de-f1c6d2a21e05" alt="Description" width="700" />

<br><br>

2. C# 비동기/대기(async/await)<br>
표준 C# 비동기/대기(async/await)를 사용하고, 동적으로 데이터를 다운로드 하여 3d model 렌더링
<img src="https://github.com/richrookie/Deep.Fine_assignment/assets/83854046/d30bcbf5-b40f-49c6-9f70-b4345e03de71" alt="Description" width="700" />

<br><br>

3. C# 비동기/대기(async/await) - 20개<br>
'2.' 과 동일한 방법을 사용하였고, 20개의 3d model을 Task.WhenAll()을 사용하여 여러 비동기 작업을 병렬로 실행
<img src="https://github.com/richrookie/Deep.Fine_assignment/assets/83854046/c734da47-c0a0-46f8-9efa-261059b5d6b0" alt="Description" width="700" />
