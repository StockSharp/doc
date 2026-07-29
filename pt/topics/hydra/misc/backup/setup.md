# Configuração

A seguir descreve-se como criar e configurar uma conta AWS.

1. Para criar uma conta AWS, aceda à página [Criar uma conta AWS](https://aws.amazon.com/ru/) e clique no botão **Criar conta**.![Captura de ecrã de Configuração 1](../../../../images/aws3_createaccount.png)
2. Em seguida, preencha os formulários que o serviço Web apresentar.
3. Preencha os dados do seu cartão; isto é feito para verificar a sua identidade.![Captura de ecrã de Configuração 2](../../../../images/aws3_paytest.png)
4. Num dos passos de registo, ser-lhe-á pedido que introduza um número de telefone e inicie uma chamada para o seu telefone através do botão **Call Me Now**.![Captura de ecrã de Configuração 3](../../../../images/aws3_callmenow.png)

   Deve atender a chamada e marcar no telefone o código que será apresentado no ecrã do computador.
5. Em seguida, ser-lhe-á pedido que selecione um plano de suporte. Após concluir a criação da conta, deve ir para a consola de gestão.![Captura de ecrã de Configuração 4](../../../../images/aws3_console.png)
6. O primeiro passo na configuração de uma conta é criar um Bucket.![AWS S3 criar bucket](../../../../images/aws3_createbucket.png)

   Bucket é um contentor para armazenar objetos na nuvem. Para o Bucket, deve definir um nome único e também selecionar um centro de dados regional (Region) onde os dados são fisicamente armazenados. Tenha em atenção que, mais tarde, ao configurar a tarefa de cópia de segurança: 1) no campo **Armazenamento**, terá de introduzir o nome do bucket; 2) no campo **Endereço**, deve utilizar não o nome, mas o endereço do centro de dados regional, que pode ser encontrado [aqui](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region). Depois continue a configuração.![AWS S3 criar bucket](../../../../images/aws3_createbucket.png)![AWS S3 nome do bucket](../../../../images/aws3_createbucketname.png)![AWS S3 propriedade do nome do bucket](../../../../images/aws3_createbucketname_propert.png)
7. Em seguida, deve configurar chaves para aceder programaticamente aos serviços AWS. Para o fazer, vá para a ligação **Security Credentials** na consola AWS.![Captura de ecrã de Configuração 5](../../../../images/aws3_securitycredentials.png)
8. Expanda o cabeçalho **Access Keys (Access Key ID and Secret Access Key)** e crie chaves de acesso utilizando o botão ![Captura de ecrã de Configuração 6](../../../../images/aws3_createnewaccesskey.png).![Captura de ecrã de Configuração 7](../../../../images/aws3_securitycredentialscreate.png)

  As chaves criadas podem ser guardadas num ficheiro utilizando o botão **Download Key File**.

   Tenha em atenção que, ao configurar uma tarefa de cópia de segurança, o **Access Key ID** deve ser utilizado como início de sessão e a **Secret Access Key** como palavra-passe.

## Conteúdo recomendado

[Criar e configurar uma tarefa](hydra_settings.md)
