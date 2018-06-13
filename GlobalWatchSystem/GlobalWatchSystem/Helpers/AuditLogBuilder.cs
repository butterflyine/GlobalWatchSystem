using System;
using GlobalWatchSystem.Models;

namespace GlobalWatchSystem.Helpers
{
    public class AuditLogBuilder
    {
        private AuditAction action;
        private string actioner;
        private string changes = "";
        private String target;
        private string type;

        private AuditLogBuilder()
        {
        }

        public static AuditLogBuilder Builder()
        {
            return new AuditLogBuilder();
        }

        public AuditLog Build()
        {
            return new AuditLog
            {
                Action = action.ToString(),
                Actioner = actioner,
                Changes = changes,
                Dttm = DateTime.Now,
                Type = type,
                Target = target
            };
        }

        public AuditLogBuilder Added(Type type, string target)
        {
            action = AuditAction.Add;
            this.target = target;
            this.type = type.Name;
            return this;
        }

        public AuditLogBuilder Updated(Type type, string target)
        {
            action = AuditAction.Update;
            this.target = target;
            this.type = type.Name;
            return this;
        }

        public AuditLogBuilder Deleted(Type type, string target)
        {
            action = AuditAction.Delete;
            this.target = target;
            this.type = type.Name;
            return this;
        }

        public AuditLogBuilder User(String who)
        {
            actioner = who;
            return this;
        }

        public AuditLogBuilder With(String changeInfo)
        {
            this.changes = changeInfo;
            return this;
        }

        private enum AuditAction
        {
            Add,
            Update,
            Delete
        }
    }
}