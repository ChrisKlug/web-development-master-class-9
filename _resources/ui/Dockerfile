FROM oven/bun:1 AS build

WORKDIR /app

COPY package.json package.json
COPY bun.lockb bun.lockb

RUN bun install

COPY . .

RUN bun run build

FROM nginx:alpine

COPY --from=build /app/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/build /usr/share/nginx/html

ENV PORT=80

# Expose the default nginx port
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]