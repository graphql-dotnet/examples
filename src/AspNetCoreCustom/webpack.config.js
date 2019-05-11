var webpack = require('webpack')
var path = require('path')
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = [
  {
    entry: {
      'bundle': './app/app.js',
    },

    output: {
      path: path.resolve('./Example/public'),
      filename: '[name].js'
    },

    resolve: {
      extensions: ['*', '.mjs', '.js', '.json', '.gql', '.graphql']
    },

    module: {
      rules: [
        // fixes https://github.com/graphql/graphql-js/issues/1272
        {
          test: /\.mjs$/,
          include: /node_modules/,
          type: 'javascript/auto'
        },
        {
          test: /\.m?js$/,
          exclude: /(node_modules|bower_components)/,
          use: {
            loader: 'babel-loader',
            options: {
              presets: ['@babel/preset-env']
            }
          }
        },
        { test: /\.flow/, use: 'ignore-loader' },
        {
          test: /\.css$/,
          use: [
            {
              loader: MiniCssExtractPlugin.loader,
              options: {
                // you can specify a publicPath here
                // by default it uses publicPath in webpackOptions.output
                publicPath: '../',
                hmr: process.env.NODE_ENV === 'development',
              },
            },
            'css-loader',
          ],
        },
      ]
    },

    plugins: [
      new MiniCssExtractPlugin({
        // Options similar to the same options in webpackOptions.output
        // both options are optional
        filename: '[name].css',
        chunkFilename: '[id].css',
      })
    ]
  }
]
