require "win32/service"

include Win32

SERVICE_NAME = "ruby_parts_server"

# Delete the service
# NOTE: if the services applet is up during this operation, the service won"t be removed from that UI
# until it is closed and reopened (it gets marked for deletion)
Service.delete(SERVICE_NAME)
