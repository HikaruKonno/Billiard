using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(ParticleSystemRenderer))]
public class Beam : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    [Header("Appearance")]
    [Min(0.001f)] public float thickness = 0.06f; // 太さ（見た目の直径）
    [Min(0.001f)] public float minVisibleLength = 0.01f;
    [Min(0f)] public float endBleed = 0.15f; // 両端の突き出し量（ワールド単位）
                                             // 任意：端キャップ（小さな発光QuadやPSをアサイン）
    public Transform capA;
    public Transform capB;
    public float tilesPerUnit = 1.0f; // 長さに対するテクスチャ繰り返し密度（シェーダ側対応推奨）
    public float capScale = 1.2f; // thicknessに対する倍率

    ParticleSystem ps;
    ParticleSystemRenderer psr;
    static Mesh crossMesh; // 使い回し

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        psr = GetComponent<ParticleSystemRenderer>();

        // Main
        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.scalingMode = ParticleSystemScalingMode.Local; // 親スケールの影響を避けるため、親は(1,1,1)推奨
        main.startSpeed = 0f;
        main.startLifetime = 999999f;
        main.loop = false;
        main.playOnAwake = false;
        main.maxParticles = 1;
        main.startSize3D = true;
        main.startSizeX = 1f; // 厚みはTransformのX/Yで出すので1
        main.startSizeY = 1f;
        main.startSizeZ = 1f; // 長さはTransformのZで出すので1

        // Emission/Shape
        var emission = ps.emission;
        emission.enabled = false; // 自動放出なし（手動Emitのみ）

        var shape = ps.shape;
        shape.enabled = false; // 形状は使わない

        // Renderer（Meshモード）
        psr.renderMode = ParticleSystemRenderMode.Mesh;
        psr.alignment = ParticleSystemRenderSpace.Local; // ローカル基準
        psr.enableGPUInstancing = true;
        psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        psr.receiveShadows = false;
        psr.sortMode = ParticleSystemSortMode.None;

        if (crossMesh == null)
            crossMesh = BuildCrossPlaneZUnit(); // Z方向1.0の交差板メッシュ

        psr.mesh = crossMesh;

        // マテリアルは Additive/Unlit で、テクスチャUVのV方向スクロールやノイズ歪みがあると“ビームらしさ”が出る
        // 例: シェーダ側で _TilingV (長さ方向の反復) と _ScrollV をサポートすると使いやすい
    }

    void OnEnable()
    {
        ps.Clear(true);
        ps.Play(true);
        ps.Emit(1); // 1粒子だけ常駐
    }

    void LateUpdate()
    {
        if (!pointA || !pointB) return;

        Vector3 a = pointA.position;
        Vector3 b = pointB.position;
        Vector3 dir = b - a;
        float len = dir.magnitude;

        if (len < minVisibleLength)
        {
            if (ps.isPlaying) ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            // 端キャップも消したい場合はここで非表示
            return;
        }
        if (!ps.isPlaying) ps.Play();

        // 方向・中点
        transform.position = (a + b) * 0.5f;
        transform.rotation = Quaternion.LookRotation(dir);

        // ここが肝：両端を突き出す
        float targetLen = Mathf.Max(minVisibleLength, len + endBleed * 2f);
        transform.localScale = new Vector3(thickness, thickness, targetLen);

        // 端キャップ（任意）：A/Bに常時配置
        if (capA)
        {
            capA.position = a;
            capA.localScale = Vector3.one * thickness * capScale;
        }
        if (capB)
        {
            capB.position = b;
            capB.localScale = Vector3.one * thickness * capScale;
        }
    }

    // Z方向に長さ1.0、中心原点の交差板メッシュ（両面描画想定）
    Mesh BuildCrossPlaneZUnit()
    {
        var m = new Mesh();
        // 2枚の四角形（XZ平面とYZ平面に相当する“Z方向帯”）
        // 長さ方向: Z = -0.5 .. +0.5、幅方向: X or Y = -0.5 .. +0.5
        Vector3[] v = {
            // 平面1（X幅）
            new(-0.5f, 0f, -0.5f), new( 0.5f, 0f, -0.5f), new( 0.5f, 0f,  0.5f), new(-0.5f, 0f,  0.5f),
            // 平面2（Y幅）
            new(0f, -0.5f, -0.5f), new(0f,  0.5f, -0.5f), new(0f,  0.5f,  0.5f), new(0f, -0.5f,  0.5f),
        };
        // UV：U=幅方向(0..1), V=長さ方向(0..1)
        Vector2[] uv = {
            new(0,0), new(1,0), new(1,1), new(0,1),
            new(0,0), new(1,0), new(1,1), new(0,1),
        };
        int[] idx = {
            0,2,1, 0,3,2,  // 平面1
            4,6,5, 4,7,6   // 平面2
        };
        // 法線はLookRotationで向けるので、ここでは概ねZ軸周り両面を想定（シェーダは両面表示推奨）
        m.SetVertices(v);
        m.SetUVs(0, new System.Collections.Generic.List<Vector2>(uv));
        m.SetTriangles(idx, 0);
        m.RecalculateNormals();
        m.RecalculateBounds();
        return m;
    }
}
