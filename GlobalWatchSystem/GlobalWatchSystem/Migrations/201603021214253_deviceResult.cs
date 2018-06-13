namespace GlobalWatchSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deviceResult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceParams",
                c => new
                    {
                        DeviceResultId = c.Int(nullable: false),
                        dev_channel = c.Int(nullable: false),
                        data_type = c.Int(nullable: false),
                        data_value = c.Single(nullable: false),
                        DeviceResult_DeviceResultId = c.Int(),
                    })
                .PrimaryKey(t => t.DeviceResultId)
                .ForeignKey("dbo.DeviceResults", t => t.DeviceResult_DeviceResultId)
                .Index(t => t.DeviceResult_DeviceResultId);
            
            CreateTable(
                "dbo.DeviceResults",
                c => new
                    {
                        DeviceResultId = c.Int(nullable: false, identity: true),
                        IMEI = c.String(maxLength: 32),
                        RecvTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceResultId)
                .Index(t => t.IMEI)
                .Index(t => t.RecvTime);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceParams", "DeviceResult_DeviceResultId", "dbo.DeviceResults");
            DropIndex("dbo.DeviceResults", new[] { "RecvTime" });
            DropIndex("dbo.DeviceResults", new[] { "IMEI" });
            DropIndex("dbo.DeviceParams", new[] { "DeviceResult_DeviceResultId" });
            DropTable("dbo.DeviceResults");
            DropTable("dbo.DeviceParams");
        }
    }
}
