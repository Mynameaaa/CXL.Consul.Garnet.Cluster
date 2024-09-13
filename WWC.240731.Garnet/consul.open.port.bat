netsh advfirewall firewall add rule name="Open Port 8500" dir=in action=allow protocol=TCP localport=8500
netsh advfirewall firewall add rule name="Open Port 8500" dir=in action=allow protocol=UDP localport=8500
netsh advfirewall firewall add rule name="Open Port 8503" dir=in action=allow protocol=TCP localport=8503
netsh advfirewall firewall add rule name="Open Port 8503" dir=in action=allow protocol=UDP localport=8503
netsh advfirewall firewall add rule name="Open Port 8600" dir=in action=allow protocol=TCP localport=8600
netsh advfirewall firewall add rule name="Open Port 8600" dir=in action=allow protocol=UDP localport=8600
netsh advfirewall firewall add rule name="Open Port 8300" dir=in action=allow protocol=TCP localport=8300
netsh advfirewall firewall add rule name="Open Port 8300" dir=in action=allow protocol=UDP localport=8300
netsh advfirewall firewall add rule name="Open Port 8301" dir=in action=allow protocol=TCP localport=8301
netsh advfirewall firewall add rule name="Open Port 8301" dir=in action=allow protocol=UDP localport=8301
netsh advfirewall firewall add rule name="Open Port 8302" dir=in action=allow protocol=TCP localport=8302
netsh advfirewall firewall add rule name="Open Port 8302" dir=in action=allow protocol=UDP localport=8302

sudo firewall-cmd --permanent --add-port=8500/tcp
sudo firewall-cmd --permanent --add-port=8500/udp
sudo firewall-cmd --permanent --add-port=8503/tcp
sudo firewall-cmd --permanent --add-port=8503/udp
sudo firewall-cmd --permanent --add-port=8600/tcp
sudo firewall-cmd --permanent --add-port=8600/udp
sudo firewall-cmd --permanent --add-port=8300/tcp
sudo firewall-cmd --permanent --add-port=8300/udp
sudo firewall-cmd --permanent --add-port=8501/tcp
sudo firewall-cmd --permanent --add-port=8501/udp
sudo firewall-cmd --permanent --add-port=8302/tcp
sudo firewall-cmd --permanent --add-port=8302/udp

netsh advfirewall firewall add rule name="Open Port 5001" dir=in action=allow protocol=TCP localport=5001
netsh advfirewall firewall add rule name="Open Port 5001" dir=in action=allow protocol=UDP localport=5001


netsh advfirewall firewall add rule name="Open Port 8510" dir=in action=allow protocol=TCP localport=8510
netsh advfirewall firewall add rule name="Open Port 8510" dir=in action=allow protocol=UDP localport=8510
netsh advfirewall firewall add rule name="Open Port 8513" dir=in action=allow protocol=TCP localport=8513
netsh advfirewall firewall add rule name="Open Port 8513" dir=in action=allow protocol=UDP localport=8513
netsh advfirewall firewall add rule name="Open Port 8610" dir=in action=allow protocol=TCP localport=8610
netsh advfirewall firewall add rule name="Open Port 8610" dir=in action=allow protocol=UDP localport=8610
netsh advfirewall firewall add rule name="Open Port 8310" dir=in action=allow protocol=TCP localport=8310
netsh advfirewall firewall add rule name="Open Port 8310" dir=in action=allow protocol=UDP localport=8310
netsh advfirewall firewall add rule name="Open Port 8311" dir=in action=allow protocol=TCP localport=8311
netsh advfirewall firewall add rule name="Open Port 8311" dir=in action=allow protocol=UDP localport=8311
netsh advfirewall firewall add rule name="Open Port 8312" dir=in action=allow protocol=TCP localport=8312
netsh advfirewall firewall add rule name="Open Port 8312" dir=in action=allow protocol=UDP localport=8312


sudo firewall-cmd --permanent --add-port=8510/tcp
sudo firewall-cmd --permanent --add-port=8510/udp
sudo firewall-cmd --permanent --add-port=8513/tcp
sudo firewall-cmd --permanent --add-port=8513/udp
sudo firewall-cmd --permanent --add-port=8610/tcp
sudo firewall-cmd --permanent --add-port=8610/udp
sudo firewall-cmd --permanent --add-port=8310/tcp
sudo firewall-cmd --permanent --add-port=8310/udp
sudo firewall-cmd --permanent --add-port=8511/tcp
sudo firewall-cmd --permanent --add-port=8511/udp
sudo firewall-cmd --permanent --add-port=8312/tcp
sudo firewall-cmd --permanent --add-port=8312/udp

  
netsh advfirewall firewall add rule name="Open Port 8520" dir=in action=allow protocol=TCP localport=8520
netsh advfirewall firewall add rule name="Open Port 8520" dir=in action=allow protocol=UDP localport=8520
netsh advfirewall firewall add rule name="Open Port 8523" dir=in action=allow protocol=TCP localport=8523
netsh advfirewall firewall add rule name="Open Port 8523" dir=in action=allow protocol=UDP localport=8523
netsh advfirewall firewall add rule name="Open Port 8620" dir=in action=allow protocol=TCP localport=8620
netsh advfirewall firewall add rule name="Open Port 8620" dir=in action=allow protocol=UDP localport=8620
netsh advfirewall firewall add rule name="Open Port 8320" dir=in action=allow protocol=TCP localport=8320
netsh advfirewall firewall add rule name="Open Port 8320" dir=in action=allow protocol=UDP localport=8320
netsh advfirewall firewall add rule name="Open Port 8321" dir=in action=allow protocol=TCP localport=8321
netsh advfirewall firewall add rule name="Open Port 8321" dir=in action=allow protocol=UDP localport=8321
netsh advfirewall firewall add rule name="Open Port 8322" dir=in action=allow protocol=TCP localport=8322
netsh advfirewall firewall add rule name="Open Port 8322" dir=in action=allow protocol=UDP localport=8322

sudo firewall-cmd --permanent --add-port=8520/tcp
sudo firewall-cmd --permanent --add-port=8520/udp
sudo firewall-cmd --permanent --add-port=8523/tcp
sudo firewall-cmd --permanent --add-port=8523/udp
sudo firewall-cmd --permanent --add-port=8620/tcp
sudo firewall-cmd --permanent --add-port=8620/udp
sudo firewall-cmd --permanent --add-port=8320/tcp
sudo firewall-cmd --permanent --add-port=8320/udp
sudo firewall-cmd --permanent --add-port=8521/tcp
sudo firewall-cmd --permanent --add-port=8521/udp
sudo firewall-cmd --permanent --add-port=8322/tcp
sudo firewall-cmd --permanent --add-port=8322/udp

pause