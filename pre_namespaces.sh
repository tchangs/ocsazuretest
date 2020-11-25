docker_config="C:\Users\timc\.docker\config.json"
echo $docker_config

ns_oncone=${1:-"ocone"}
ns_monitoring=${2:-"monitoring"}
ns_summit=${3:-"summit"}
echo "ocone namespace=$ns_oncone"
echo "monitoring namespace=$ns_monitoring"

kubectl create namespace $ns_oncone
kubectl create secret generic omnidocker --from-file=.dockerconfigjson=$docker_config --type=kubernetes.io/dockerconfigjson --namespace=$ns_oncone

kubectl create namespace $ns_monitoring
kubectl create secret generic omnidocker --from-file=.dockerconfigjson=$docker_config --type=kubernetes.io/dockerconfigjson --namespace=$ns_monitoring

kubectl create namespace $ns_summit
kubectl create secret generic omnidocker --from-file=.dockerconfigjson=$docker_config --type=kubernetes.io/dockerconfigjson --namespace=$ns_summit