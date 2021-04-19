using Unity.Mathematics;

sealed class OneEuroFilter2
{
    #region Public properties

    public float Beta { get; set; }
    public float MinCutoff { get; set; }

    #endregion

    #region Public step function

    public float2 Step(float t, float2 x)
    {
        var t_e = t - _prev.t;

        // Do nothing if the time difference is too small.
        if (t_e < 1e-5f) return _prev.x;

        var dx = (x - _prev.x) / t_e;
        var dx_res = math.lerp(_prev.dx, dx, Alpha(t_e, DCutOff));

        var cutoff = MinCutoff + Beta * math.length(dx_res);
        var x_res = math.lerp(_prev.x, x, Alpha(t_e, cutoff));

        _prev = (t, x_res, dx_res);

        return x_res;
    }

    #endregion

    #region Private class members

    const float DCutOff = 1.0f;

    static float Alpha(float t_e, float cutoff)
    {
        var r = 2 * math.PI * cutoff * t_e;
        return r / (r + 1);
    }

    #endregion

    #region Previous state variables as a tuple

    (float t, float2 x, float2 dx) _prev;

    #endregion
}
