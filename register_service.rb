require "win32/service"

include Win32

SERVICE_NAME = "ruby_parts_server"

# Create a new service
Service.create({
  :service_name		=> SERVICE_NAME,
  :display_name		=> "Ruby Parts Server",
  :service_type		=> Service::WIN32_OWN_PROCESS,
  :description		=> "Ruby Parts Server Windows Service",
  :start_type		=> Service::AUTO_START,
  :error_control	=> Service::ERROR_NORMAL,
  :binary_path_name	=> "C:/Ruby22/x64/bin/ruby.exe C:/PartsServer/parts_service.rb",
  :load_order_group	=> "Network",
  :dependencies		=> ["W32Time","Schedule"],
})
