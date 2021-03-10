### Setup
1. Convert Key to PublicKey format
```bash
chmod 600 Key.pem
ssh-keygen -y -f Key.pem > PublicKey.pem
```
2. Create the cluster & node group
```bash
eksctl create cluster \
--name my-cluster \
--with-oidc \
--ssh-access \
--ssh-public-key PublicKey.pem \
--managed
```
3. Create deployment
	- Create file
```bash
vim nginx-deployment.yaml
```
```vim
:set paste
i
:wq
```
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: nginx-deployment
  labels:
    app: nginx
spec:
  replicas: 2
  selector:
    matchLabels:
      app: nginx
  template:
    metadata:
      labels:
        app: nginx
    spec:
      containers:
      - name: nginx
        image: nginx:1.14.2
        ports:
        - containerPort: 80
```
	- Deploy
```bash
kubectl apply -f nginx-deployment.yaml
kubectl get deploy
```
3. Create LoadBalancer service
	- Create file
```bash
vim loadbalancer.yaml
```
```vim
:set paste
i
:wq
```
```yaml
apiVersion: v1
kind: Service
metadata:
  name: nginx-service
spec:
  type: LoadBalancer
  selector:
    app: nginx
  ports:
    - protocol: TCP
      port: 80
      targetPort: 80
```
	- Deploy
```bash
kubectl create -f loadbalancer.yaml
kubectl get svc
```
	- NOTE: An endpoint will be created for the service from the classic load balancer at https://console.aws.amazon.com/ec2/v2/home?region=us-east-1#LoadBalancers:sort=loadBalancerName. It takes a few minutes before the site will be loaded
	- Go to classic load balancer DNS (e.g. a54af8659cf724ed78b1f6d2c2d03bbe-1117220730.us-east-1.elb.amazonaws.com)

### Notes
- Fargate doesn't allow LoadBalancer external dns/ip to work. So you must do this through managed nodes.

### Resources
- https://www.youtube.com/watch?v=c4WcYjama6U
- https://aws.amazon.com/premiumsupport/knowledge-center/eks-kubernetes-services-cluster/
- https://docs.aws.amazon.com/eks/latest/userguide/getting-started-eksctl.html
