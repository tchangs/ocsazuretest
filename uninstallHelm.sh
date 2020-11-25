echo " 0. uninstall all."
echo " 1. uninstall ocone."
echo " 2. uninstall monitoring."

echo " 3. uninstall grafana."
echo " 4. uninstall prometheus."
echo " 5. uninstall locust."
echo " 6. uninstall loadtest."
echo " 7. uninstall kibana & elasticsearch."
echo " 8. uninstall fluentd."
echo " 8. uninstall distributed-jmeter."

helm_chart=${1:-0}
ns_oncone=${2:-"ocone"}
ns_monitoring=${3:-"monitoring"}
ns_summit=${4:-"summit"}

echo "args: $helm_chart $ns_oncone $ns_monitoring"

monitoring=0
if [ "$helm_chart" -eq 0 ] || [ "$helm_chart" -eq 2 ] ; then
	monitoring=1
fi

if [ "$helm_chart" -eq 0 ] || [ "$helm_chart" -eq 1 ] ; then
	echo "uninstall ocone..."
	helm uninstall ocone --namespace $ns_oncone
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 3 ] ; then
   echo "uninstall grafana..."
   helm uninstall grafana --namespace=$ns_monitoring
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 4 ] ; then
   echo "uninstall prometheus..."
   helm uninstall prometheus --namespace=$ns_monitoring
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 5 ] ; then
   echo "uninstall locust..."
   helm uninstall locust --namespace=$ns_monitoring
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 6 ] ; then
   echo "uninstall loadtest..."
   helm uninstall loadtest --namespace=$ns_monitoring
fi

if [ "$monitoring" -eq 1 ] || [ "$helm_chart" -eq 7 ] ; then
   echo "uninstall kibana & elasticsearch..."
   helm uninstall kibana --namespace=$ns_monitoring
fi

if [ "$helm_chart" -eq 0 ] || [ "$helm_chart" -eq 8 ] ; then
   echo "uninstall fluentd..."
   helm uninstall fluentd --namespace=kube-system
fi

if [ "$helm_chart" -eq 0 ] || [ "$helm_chart" -eq 9 ] ; then
	echo "uninstall distributed-jmeter..."
	helm uninstall distributed-jmeter --namespace $ns_summit
fi