# Script for running the parts management server as a Windows service.
LOG_FILE = "C:/temp/parts_service.log"

begin
  require "win32/daemon"
  
  include Win32
  
  require_relative "parts_server"
  
  class PartsDaemon < Daemon
    def service_main
      CheesyParts::Server.run! :bind => "0.0.0.0", :port => PORT, :server => "thin"
	  File.open(LOG_FILE, "a"){ |f| f.puts("#{Time.now} Service started") }
      # while running?
      #   sleep 10
      #   File.open(LOG_FILE, "a"){ |f| f.puts("#{Time.now} Service running") }
      # end
    end

    def service_stop
	  CheesyParts::Server.stop!
      File.open(LOG_FILE, "a"){ |f| f.puts("#{Time.now} Service stopped") }
    end
  end

  PartsDaemon.mainloop

rescue Exception => err
  File.open(LOG_FILE, "a"){ |f| f.puts("#{Time.now} Service failure") }
  File.open(LOG_FILE, "a"){ |f| f.puts("Error #{err}") }
  raise
end
