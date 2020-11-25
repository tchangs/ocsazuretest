echo " 0. install all."
echo " 1. install ocone."
echo " 2. install monitoring."

echo " 3. install grafana."
echo " 4. install prometheus."
echo " 5. install locust."
echo " 6. install loadtest."
echo " 7. install kibana & elasticsearch."
echo " 8. install fluentd."
echo " 8. install distributed-jmeter."

helm_chart=${1:-0}
ns_oncone=${2:-"ocone"}
ns_monitoring=${3:-"monitoring"}
ns_summit=${4:-"summit"}

echo "args: $helm_chart $ns_oncone $ns_monitoring"

if [ "$helm_chart" -eq 0 ] || [ "$helm_chart" -eq 1 ] ; then
	ssl_certifcate=${4:-"pc-workflow-web"}
	githubrepo="https://raw.githubusercontent.com/omnicellgithub/OC-One/develop/certs"
	pcworkflowwebsslcrturl="$githubrepo/$ssl_certifcate.crt"
	pcworkflowwebsslkeyurl="$githubrepo/$ssl_certifcate.key"

	minikube_ip=$(minikube ip)
	echo "minikube ip is: $minikube_ip"
	echo "github repo: $githubrepo"
	echo "pcworkflowwebsslcrturl: $pcworkflowwebsslcrturl"
	echo "pcworkflowwebsslkeyurl: $pcworkflowwebsslkeyurl"

	echo $(pwd)

	echo "install ocone in $ns_oncone namespace..."
	helm install  ocone ocone-1.0.0.tgz --namespace=$ns_oncone \
     --set kafkaAdvertizedListners=$minikube_ip \
     --set kafkaAdvertizedListnersPort=30099 \
     --set ocone.web.sslcrturl=$pcworkflowwebsslcrturl \
     --set ocone.web.sslkeyurl=$pcworkflowwebsslkeyurl \
     --set stage=dev
fi

monitoring=0
if [ "$helm_chart" -eq 0 ] || [ "$helm_chart" -eq 2 ] ; then
	monitoring=1
fi
echo "install monitoring: $monitoring"

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 3 ] ; then
   echo "install grafana..."
   helm install grafana grafana-1.0.0.tgz --namespace=$ns_monitoring
   grafanaip=$(kubectl get svc grafana -n monitoring -o yaml | grep IP | cut -d ":" -f2 | xargs )
   kubectl create secret generic grafana-creds  --from-literal=GF_SECURITY_ADMIN_USER=admin \
   --from-literal=GF_SECURITY_ADMIN_PASSWORD=omnicell \
   --namespace=monitoring
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 4 ] ; then
   echo "install prometheus..."
   minikube addons enable metrics-server
   helm repo add stable https://kubernetes-charts.storage.googleapis.com/
   helm repo update
   helm install prometheus stable/prometheus -f Monitoring/prometheus/prometheus.yml --namespace $ns_monitoring
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 5 ] ; then
   echo "install locust..."
   hostip=$(kubectl get svc pc-workflow-web -n ocone -o yaml | grep IP | cut -d ":" -f2 | xargs )
   grafanaip=$(kubectl get svc grafana -n monitoring -o yaml | grep IP | cut -d ":" -f2 | xargs )
   # prometheusip=$(kubectl get svc prometheus-server -n monitoring -o yaml | grep IP | cut -d ":" -f2 | grep 10 xargs )
   helm install locust locust-0.1.0.tgz  \
   --set targetHost=https://ghs.kube.myomnicell.com:8080  \
   --set targetHostIP=$hostip  \
   --set grafanaHostIP=$grafanaip  \
   -n $ns_monitoring
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 6 ] ; then
   echo "install loadtest..."
   helm install loadtest loadtest-0.1.0.tgz -n $ns_monitoring
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 7 ] ; then
   echo "install kibana..."
   helm install kibana kibana-1.0.0.tgz -n $ns_monitoring
fi

if [ "$helm_chart" -eq 0 ] || [ "$helm_chart" -eq 8 ] ; then
   echo "install fluentd into kube-system..."
   helm install fluentd stable/fluentd-elasticsearch -f Monitoring/fluentd/values.yaml -n kube-system
fi

if [ "$helm_chart" -eq 0 ] || [ "$helm_chart" -eq 9 ] ; then
   echo "install distributed-jmeter..."
   helm install  distributed-jmeter distributed-jmeter-1.0.1.tgz -n $ns_monitoring
fi