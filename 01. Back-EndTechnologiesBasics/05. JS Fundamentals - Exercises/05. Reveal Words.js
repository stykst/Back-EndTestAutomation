function replaceWords(wordString, templateString) {
    const words = wordString.split(', ');
    const templates = templateString.split(' ');

    const replacedWords = templates.map(template => {
        if (template.includes('*')) {
            const length = template.length;
            const matchingWord = words.find(word => word.length === length);
            return matchingWord || template;
        } else {
            return template;
        }
    });

    const result = replacedWords.join(' ');
    return result;
}