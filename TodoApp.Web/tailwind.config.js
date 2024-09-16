/** @type {import('tailwindcss').Config} */

module.exports = {
  content: [
    './Pages/**/*.{razor,html,cshtml}',
    './Components/**/*.{razor,html,cshtml}',
    './wwwroot/**/*.html',
  ],
  theme: {
    extend: {},
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}