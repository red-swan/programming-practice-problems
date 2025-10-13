#lang racket



; Environment setup -------------------------------------------------------------
(require racket/hash)

(define DEV #f)
(define input-path (make-parameter "04.txt"))
(when DEV (input-path "04-sample.txt"))


; Hash Functions ----------------------------------------------------------------

(define (hash-update-keys ht updater)
  (for/hash ([k (hash-keys ht)])
    (values (updater k) (hash-ref ht k))))

(define (hash-intersection ht1 ht2)
  (for/hash ([key1 (hash-keys ht1)]
             #:when (and (hash-has-key? ht2 key1)
                     (equal? (hash-ref ht1 key1)
                             (hash-ref ht2 key1))))
    (values key1 (hash-ref ht1 key1))))

(define (hash-subset? ht ht2)
  (for/fold ([still-subset? #t])
            ([key2 (hash-keys ht2)])
    (if still-subset?
        (and (hash-has-key? ht key2)
             (equal? (hash-ref ht key2)
                     (hash-ref ht2 key2)))
        still-subset?)))

(define (hash-has-same-key-value? ht1 ht2 key)
  (and (hash-has-key? ht1)
       (hash-has-key? ht2)
       (equal? (hash-ref ht1 key)
               (hash-ref ht2 key))))

(define (hash-difference ht ht2)
  (hash-filter ht
               (λ (k v) (not
                         (and
                          (hash-has-key? ht2 k)
                          (equal? v (hash-ref ht2 k)))))))

; Coordinate Functions ----------------------------------------------------------

;Add x y coordinates stored as lists
(define (add pt1 pt2)
  (match-let ([(list x1 y1) pt1]
              [(list x2 y2) pt2])
    (list (+ x1 x2) (+ y1 y2))))


; Build directions and move from one to another
(define directions '(N S E W NE NW SW SE))
(define (step from dir)
  (match-let ([(list x y) from])
    (match dir
      ['N  (list       x  (sub1 y))]
      ['S  (list       x  (add1 y))]
      ['E  (list (add1 x)       y)]
      ['W  (list (sub1 x)       y)]
      ['NW (list (sub1 x) (sub1 y))]
      ['NE (list (add1 x) (sub1 y))]
      ['SW (list (sub1 x) (add1 y))]
      ['SE (list (add1 x) (add1 y))])))
(define (turn-around dir)
  (match dir
    ['N 'S]
    ['S 'N]
    ['E 'W]
    ['W 'E]
    ['NW 'SE]
    ['SW 'NE]
    ['SE 'NW]
    ['NE 'SW]))


; Word Search Functions ---------------------------------------------------------
; build a mapping from coordinate to letter
(define (build-search-mapping path)
  (for/fold ([search (hash)]
             [y 0]
             #:result search)
            ([line (file->lines path)])
    (for/fold ([search search]
               [x 0]
               #:result (values search (add1 y)))
              ([char (string->list line)])
      (values
       (hash-set search (list x y) char)
       (add1 x)))))

; Build patterns for words in a direction
(define (build-word-pattern dir word [start (list 0 0)])
  (for/fold ([at start]
             [indices (hash)]
             #:result indices)
            ([char (string->list word)])
    (values
     (step at dir)
     (hash-set indices at char))))

(define (build-word-patterns word)
  (map (curryr build-word-pattern word) directions))


; Searching for patterns in a word search
(define (hash-equal-at? ht1 ht2 key)
  (and
   (hash-has-key? ht1 key)
   (hash-has-key? ht2 key)
   (equal? (hash-ref ht1 key) (hash-ref ht2 key))))

(define (has-pattern-at? word-search at pattern)
  (for/fold ([errored? #f]
             #:result (not errored?))
            ([coordinate (hash-keys pattern)])
      (if errored?
          errored?
          (let ([coordinate* (add at coordinate)])
            (not
             (and
              (hash-has-key? word-search coordinate*)
              (equal?
               (hash-ref word-search coordinate*)
               (hash-ref pattern coordinate))))))))

(define (count-patterns-at word-search at patterns)
  (count (curry has-pattern-at? word-search at) patterns))

(define (count-occurrences word-search counter)
  (for/fold ([occurrences 0])
            ([coordinate (hash-keys word-search)])
    (let* ([letter (hash-ref word-search coordinate)]
           [count (counter word-search coordinate letter)])
      (+ occurrences count))))

; Computing Solutions ---------------------------------------

; Read data -----------------------------
(define word-search (build-search-mapping (input-path)))

; Part 1 --------------------------------
(define xmas-patterns (build-word-patterns "XMAS"))
  
(define (count-xmas-patterns word-search coordinate letter)
  (if (equal? letter #\X)
      (count-patterns-at word-search coordinate xmas-patterns)
      0))

(count-occurrences word-search count-xmas-patterns)

; Part 2 --------------------------------

(define cross-mass-patterns
  (let* ([dirs-of-interest  '((SE SW) (SW NW) (NW NE) (NE SE))]
         [f (λ (dir) (build-word-pattern (turn-around dir) "MAS" (step (list 0 0) dir)))]
         [g (λ (dirs) (apply hash-union (map f dirs) #:combine (λ (a b) a)))])
    (map g dirs-of-interest)))

(define (count-cross-mass-patterns word-search coordinate letter)
  (if (equal? letter #\A)
      (count-patterns-at word-search coordinate cross-mass-patterns)
      0))
  
(count-occurrences word-search count-cross-mass-patterns)




; Scratch ---------------------------

