namespace Microsoft.Web.Util {
    using System;

    // This type is needed to satisfy FxCop Rule CA908: UseApprovedGenericsForPrecompiledAssemblies
    internal struct NGenWrapper<T> where T : struct {
        public T Value;

        public NGenWrapper(T value) {
            this.Value = value;
        }

        public static implicit operator T(NGenWrapper<T> value) {
            return value.Value;
        }

        public static implicit operator NGenWrapper<T>(T value) {
            return new NGenWrapper<T>(value);
        }
    }
}
