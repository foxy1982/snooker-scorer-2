FROM mono:latest
ADD ./snooker-scorer/snooker-scorer /opt/snooker-scorer

EXPOSE 1147

CMD [ "mono", "/opt/snooker-scorer/bin/Release/snooker-scorer.exe", "-d"]
