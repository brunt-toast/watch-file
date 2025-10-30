autoload bashcompinit
bashcompinit
autoload -U compinit
compinit 
source script/watch-file-completion.bash

_watch-file_completions() {
    local cur="${COMP_WORDS[COMP_CWORD]}"

    local suggestions="$(watch-file [suggest] "$cur" 2>/dev/null)"
    local custom=($(compgen -W "$(tr '\n' ' ' <<<"$suggestions")" -- "$cur"))
    local files=($(compgen -f -- "$cur"))

    COMPREPLY=("${custom[@]}" "${files[@]}")
}

complete -F _watch-file_completions watch-file

