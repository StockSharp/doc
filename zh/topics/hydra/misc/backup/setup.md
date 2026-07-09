# 设置

下面介绍如何创建和配置 AWS 账户。

1. 要创建 AWS 账户，请打开[创建 AWS 账户](https://aws.amazon.com/ru/)页面，然后单击 **创建账户** 按钮。![Aws3 CreateAccount](../../../../images/aws3_createaccount.png)
2. 接下来，填写 Web 服务提供的表单。
3. 填写银行卡信息，此步骤用于验证身份。![Aws 3 Paytest](../../../../images/aws3_paytest.png)
4. 在注册过程的某个步骤中，系统会要求输入电话号码，并使用 **Call Me Now** 按钮呼叫该号码。![Aws3 CallMeNow](../../../../images/aws3_callmenow.png)

   接听电话，然后在手机上输入计算机屏幕中显示的代码。
5. 接下来，系统会要求选择支持方案。账户创建完成后，打开管理控制台。![Aws3 console](../../../../images/aws3_console.png)
6. 配置账户的第一步是创建 Bucket。![Aws3 CreateBucket](../../../../images/aws3_createbucket.png)

   Bucket 是用于在云端存储对象的容器。需要为 Bucket 设置唯一名称，并选择数据实际存储所在的区域数据中心（Region）。之后配置备份任务时请注意：1）在 **存储** 字段中输入 Bucket 名称；2）在 **地址** 字段中不要使用区域名称，而应使用区域数据中心的地址，该地址可在[此处](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region)查询。然后继续配置。![Aws3 CreateBucket](../../../../images/aws3_createbucket.png)![Aws 3 Create Bucket Name](../../../../images/aws3_createbucketname.png)![Aws 3 Create Bucket Name property](../../../../images/aws3_createbucketname_propert.png)
7. 接下来，需要设置用于以编程方式访问 AWS 服务的密钥。为此，请在 AWS 控制台中打开 **Security Credentials**。![Aws3 SecurityCredentials](../../../../images/aws3_securitycredentials.png)
8. 展开 **Access Keys (Access Key ID and Secret Access Key)**，然后使用 ![Aws3 CreateNewAccessKey](../../../../images/aws3_createnewaccesskey.png) 按钮创建访问密钥。![Aws3 SecurityCredentialsCreate](../../../../images/aws3_securitycredentialscreate.png)

   可以使用 **Download Key File** 按钮将创建的密钥保存到文件。

   配置备份任务时，请将 **Access Key ID** 用作登录名，将 **Secret Access Key** 用作密码。

## 推荐内容

[创建和配置任务](hydra_settings.md)
