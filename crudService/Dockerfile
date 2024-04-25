FROM python:3.11.3-slim-buster

# Set the working directory
WORKDIR /app

# set environment variables
# prevents writing pyc to disc
ENV PYTHONDONTWRITEBYTECODE 1
# prevents burffering stdout and stderr
ENV PYTHONUNBUFFERED 1

# install dependencies
RUN pip install --upgrade pip
COPY requirements.txt .
RUN pip install -r requirements.txt

# copy project
COPY . .

# run application with gunicorn
CMD ["gunicorn", "--bind", "0.0.0.0:5000", "wsgi:app"]
