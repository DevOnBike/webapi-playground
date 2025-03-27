cls
kubectl apply -f k8s.deploy-statefull.yaml
kubectl describe svc kubernetes-devonbike-webapi-statefull -n kubernetes-devonbike-webapi-statefull
kubectl get deploy -n kubernetes-devonbike-webapi-statefull
kubectl get pods -n kubernetes-devonbike-webapi-statefull
kubectl describe deployments -n kubernetes-devonbike-webapi-statefull
kubectl port-forward -n kubernetes-devonbike-webapi-statefull service/kubernetes-devonbike-webapi-statefull 8500:8500