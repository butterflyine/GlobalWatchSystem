using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlobalWatchSystem.Helpers
{
    public class ChangeInfo
    {
        private static readonly List<Type> CHANGES_SUPPORTED = new List<Type> {typeof (int), typeof (string), typeof (double), typeof (float), typeof (bool)};

        private readonly JObject changes = new JObject();

        public ChangeInfo AddChange(Expression<Func<object>> expression)
        {
            ChangedField change = GetChangedField(expression);
            if (change.Value != null && CHANGES_SUPPORTED.Contains(change.Value.GetType()))
            {
                changes.Add(change.Name, new JValue(change.Value));
            }
            return this;
        }

        public String ToJson()
        {
            return JsonConvert.SerializeObject(changes);
        }

        private static ChangedField GetChangedField<TU>(Expression<Func<TU>> expression)
        {
            return new ChangedField
            {
                Name = ReflectionUtils.GetMemberInfo(expression).Member.Name,
                Value = expression.Compile().Invoke()
            };
        }

        private class ChangedField
        {
            public String Name { get; set; }
            public object Value { get; set; }
        }
    }
}