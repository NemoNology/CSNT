extends LineEdit

func _ready():
	text = "192.168.0.0"
	var interfaces = IP.get_local_interfaces()
	for dict in interfaces:
		if dict["friendly"] == "Ethernet":
			text = dict["addresses"][1]
			return
