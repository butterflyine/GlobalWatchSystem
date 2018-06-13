/*初始化管理员，用户名admin，密码admin*/
INSERT INTO [dbo].[Areas] ([Name], [Description], [ParentId]) VALUES(N'全部区域(Top Areas)', N'全部区域(Top Areas)',0);
INSERT INTO [dbo].[Users] ([UserName], [Email], [Phone], [Password],[AreaId]) VALUES (N'admin', N'admin@admin.company', N'111111111111', N'1000:UlsSIUw0V39MDMkxStxGYojPPx4bl8kW:jp8va9lBBP7D78yhVNipYeBNvLDUBvaI', (SELECT TOP(1) Id FROM [dbo].[Areas]));
INSERT INTO [dbo].[AppSetting] ([TemperatureLower], [TemperatureUpper], [HumidityLower],[HumidityUpper], [Battery] ) VALUES (0.0, 20.0, 0.0,100.0,0.0)

INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x01,N'mPa', N'压力', N'Pressure');
INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x02,N'bar', N'压力', N'Pressure');
INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x03,N'kPa', N'压力', N'Pressure');

INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x04,N'm', N'液位', N'Liquid Lever');
INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x05,N'℃', N'温度', N'Temperature');
INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x06,N'%RH', N'湿度', N'Humidity');

INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x07,N'm³/h', N'流量', N'Flow');
INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x08,N'm³/min', N'流量', N'Flow');

INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x09,N'V', N'电压', N'Voltage');
INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x0a,N'A', N'电流', N'Current');
INSERT INTO [dbo].[DeviceUnits] ([UnitCode], [UnitName], [UnitDesc_CN], [UnitDesc_EN]) values(0x0b,N'kWh', N'功率', N'Power');

INSERT INTO [dbo].[DeviceMetas] ([DeviceType], [CanLocation], [MetaContent], [CreateTime], [ModifyTime]) values(N'12路温度仪', true, N'1,5;2,5;3,5;4,5;5,5;6,5;7,5;8,5;9,5;10,5;11,5;12,5;', GetDate(),null);
