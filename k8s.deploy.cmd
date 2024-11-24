cls
kubectl apply -f k8s.deploy.yaml
kubectl describe svc kubernetes-devonbike-webapi -n kubernetes-devonbike-webapi
kubectl get deploy -n kubernetes-devonbike-webapi
kubectl get pods -n kubernetes-devonbike-webapi
kubectl describe deployments -n kubernetes-devonbike-webapi
kubectl port-forward -n kubernetes-devonbike-webapi service/kubernetes-devonbike-webapi 8500:8500