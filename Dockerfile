FROM node:22.12-alpine AS builder

WORKDIR /app

COPY src /app
COPY bridge_config.json /bridge_config.json

RUN --mount=type=cache,target=/root/.npm npm install
RUN --mount=type=cache,target=/root/.npm-production npm ci --ignore-scripts --omit-dev

RUN npm install -g @modelcontextprotocol/server-filesystem

FROM node:22-alpine AS release

WORKDIR /app

COPY --from=builder /app/dist /app/dist
COPY --from=builder /app/package.json /app/package.json
COPY --from=builder /app/package-lock.json /app/package-lock.json
COPY --from=builder /usr/lib/node_modules/@modelcontextprotocol /usr/lib/node_modules/@modelcontextprotocol

ENV NODE_ENV=production

RUN npm ci --ignore-scripts --omit-dev

ENTRYPOINT ["node", "/app/dist/index.js"]
