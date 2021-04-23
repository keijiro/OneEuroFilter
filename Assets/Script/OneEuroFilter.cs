using System;
using Unity.Mathematics;

public sealed class OneEuroFilter : OneEuroFilterBase<float>
{
    public OneEuroFilter(float beta = 0.0f, float minCutoff = 1.0f) : base(beta, minCutoff) { }

    #region Public step function

    public override float Step(float t, float x)
    {
        var t_e = t - _prev.t;

        // Do nothing if the time difference is too small.
        if (t_e < MinTimeDelta) return _prev.x;

        var dx = (x - _prev.x) / t_e;
        var dx_res = math.lerp(_prev.dx, dx, Alpha(t_e, DCutOff));

        var cutoff = MinCutoff + Beta * math.length(dx_res);
        var x_res = math.lerp(_prev.x, x, Alpha(t_e, cutoff));

        _prev = (t, x_res, dx_res);

        return x_res;
    }

    #endregion
}

public sealed class OneEuroFilter2 : OneEuroFilterBase<float2>
{
    public OneEuroFilter2(float beta = 0.0f, float minCutoff = 1.0f) : base(beta, minCutoff) { }

    #region Public step function

    public override float2 Step(float t, float2 x)
    {
        var t_e = t - _prev.t;

        // Do nothing if the time difference is too small.
        if (t_e < MinTimeDelta) return _prev.x;

        var dx = (x - _prev.x) / t_e;
        var dx_res = math.lerp(_prev.dx, dx, Alpha(t_e, DCutOff));

        var cutoff = MinCutoff + Beta * math.length(dx_res);
        var x_res = math.lerp(_prev.x, x, Alpha(t_e, cutoff));

        _prev = (t, x_res, dx_res);

        return x_res;
    }

    #endregion
}

public sealed class OneEuroFilter3 : OneEuroFilterBase<float3>
{
    public OneEuroFilter3(float beta = 0.0f, float minCutoff = 1.0f) : base(beta, minCutoff) { }

    #region Public step function

    public override float3 Step(float t, float3 x)
    {
        var t_e = t - _prev.t;

        // Do nothing if the time difference is too small.
        if (t_e < MinTimeDelta) return _prev.x;

        var dx = (x - _prev.x) / t_e;
        var dx_res = math.lerp(_prev.dx, dx, Alpha(t_e, DCutOff));

        var cutoff = MinCutoff + Beta * math.length(dx_res);
        var x_res = math.lerp(_prev.x, x, Alpha(t_e, cutoff));

        _prev = (t, x_res, dx_res);

        return x_res;
    }

    #endregion
}

public sealed class OneEuroFilterQuaternion : OneEuroFilterBase<quaternion>
{
    public OneEuroFilterQuaternion(float beta = 0.0f, float minCutoff = 1.0f) : base(beta, minCutoff) {
        _prev = (0, quaternion.identity, quaternion.identity);
    }

    #region Public step function

    public override quaternion Step(float t, quaternion x)
    {
        var t_e = t - _prev.t;

        // Do nothing if the time difference is too small.
        if (t_e < MinTimeDelta) return _prev.x;

        // derived from the VRPN OneEuro quaternion filter
        var rate = 1.0f / t_e;

        var inverse_prev = math.inverse(_prev.x);
        var dx = math.mul(x, inverse_prev);

        dx.value.xyzw *= rate;
        dx.value.w += 1.0f - rate;
        dx = math.normalize(dx);

        var cutoff = MinCutoff + Beta * 2.0f * math.acos(dx.value.w);
        var x_res = math.slerp(_prev.x, x, Alpha(t_e, cutoff));

        _prev = (t, x_res, dx);

        return x_res;
    }

    #endregion
}

public abstract class OneEuroFilterBase<T> where T : IEquatable<T>
{
    #region Public properties

    public float Beta { get; set; }
    public float MinCutoff { get; set; }

    #endregion

    #region Public step function

    public abstract T Step(float t, T x);

    #endregion

    protected OneEuroFilterBase(float beta, float minCutoff)
    {
        Beta = beta;
        MinCutoff = minCutoff;
    }

    protected OneEuroFilterBase() { }

    #region Protected class members

    protected const float DCutOff = 1.0f;
    protected const float MinTimeDelta = 1e-5f;

    protected static float Alpha(float t_e, float cutoff)
    {
        var r = 2 * math.PI * cutoff * t_e;
        return r / (r + 1);
    }

    #endregion

    #region Previous state variables as a tuple

    protected (float t, T x, T dx) _prev;

    #endregion
}
